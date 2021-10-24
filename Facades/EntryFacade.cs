using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Havit.Bonusario.Contracts;
using Havit.Bonusario.DataLayer.Repositories;
using Havit.Bonusario.Facades.Infrastructure.Security.Authentication;
using Havit.Bonusario.Model;
using Havit.Bonusario.Services;
using Havit.Data.Patterns.UnitOfWorks;
using Havit.Diagnostics.Contracts;
using Havit.Extensions.DependencyInjection.Abstractions;
using Microsoft.AspNetCore.Authorization;

namespace Havit.Bonusario.Facades
{
	[Service]
	[Authorize]
	public class EntryFacade : IEntryFacade
	{
		private const int PointsAvailable = 100;

		private readonly IEntryRepository entryRepository;
		private readonly IEntryMapper entryMapper;
		private readonly IUnitOfWork unitOfWork;
		private readonly IApplicationAuthenticationService applicationAuthenticationService;

		public EntryFacade(
			IEntryRepository entryRepository,
			IEntryMapper entryMapper,
			IUnitOfWork unitOfWork,
			IApplicationAuthenticationService applicationAuthenticationService)
		{
			this.entryRepository = entryRepository;
			this.entryMapper = entryMapper;
			this.unitOfWork = unitOfWork;
			this.applicationAuthenticationService = applicationAuthenticationService;
		}

		public async Task<List<EntryDto>> GetMyEntriesAsync(Dto<int> periodId, CancellationToken cancellationToken = default)
		{
			var currentEmployee = await applicationAuthenticationService.GetCurrentEmployeeAsync(cancellationToken);
			var entries = await entryRepository.GetEntriesCreatedByAsync(periodId.Value, currentEmployee.Id, cancellationToken);

			return entries.Select(e => entryMapper.MapToEntryDto(e)).ToList();
		}

		public async Task DeleteEntryAsync(Dto<int> entryId, CancellationToken cancellationToken = default)
		{
			var currentEmployee = await applicationAuthenticationService.GetCurrentEmployeeAsync(cancellationToken);
			var entry = await entryRepository.GetObjectAsync(entryId.Value, cancellationToken);

			Contract.Requires<SecurityException>(entry.CreatedById == currentEmployee.Id);

			unitOfWork.AddForDelete(entry);
			await unitOfWork.CommitAsync(cancellationToken);
		}

		public async Task<Dto<int>> CreateEntryAsync(EntryDto newEntryDto, CancellationToken cancellationToken = default)
		{
			Contract.Requires<ArgumentNullException>(newEntryDto is not null, nameof(newEntryDto));
			Contract.Requires<ArgumentException>(newEntryDto.Id == default, nameof(newEntryDto.Id));

			var currentEmployee = await applicationAuthenticationService.GetCurrentEmployeeAsync(cancellationToken);

			var pointsAssigned = await entryRepository.GetPointsAssignedSumAsync(newEntryDto.PeriodId, currentEmployee.Id, cancellationToken);
			int maxPoints = PointsAvailable - pointsAssigned;
			if (newEntryDto.Value > maxPoints)
			{
				throw new OperationFailedException($"Maximální počet přidělitelných bodů překročen. K dispozici zbývá {maxPoints} bodů");
			}

			Entry newEntry = new Entry()
			{
				CreatedBy = currentEmployee,
				CreatedById = currentEmployee.Id
			};
			entryMapper.MapFromEntryDto(newEntryDto, newEntry);

			unitOfWork.AddForInsert(newEntry);
			await unitOfWork.CommitAsync(cancellationToken);

			return Dto.FromValue(newEntry.Id);
		}

		public async Task<Dto<int>> GetMyRemainingPoints(Dto<int> periodId, CancellationToken cancellationToken = default)
		{
			var currentEmployee = await applicationAuthenticationService.GetCurrentEmployeeAsync(cancellationToken);

			var pointsAssigned = await entryRepository.GetPointsAssignedSumAsync(periodId.Value, currentEmployee.Id, cancellationToken);

			return Dto.FromValue(PointsAvailable - pointsAssigned);
		}
	}
}

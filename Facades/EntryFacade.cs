using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Havit.Bonusario.Contracts;
using Havit.Bonusario.DataLayer.Repositories;
using Havit.Bonusario.Facades.Infrastructure.Security.Authentication;
using Havit.Bonusario.Model;
using Havit.Bonusario.Services;
using Havit.Data.Patterns.UnitOfWorks;
using Havit.Diagnostics.Contracts;
using Havit.Extensions.DependencyInjection.Abstractions;
using Havit.Services.TimeServices;
using Microsoft.AspNetCore.Authorization;

namespace Havit.Bonusario.Facades
{
	[Service]
	[Authorize]
	public class EntryFacade : IEntryFacade
	{
		private const int PointsAvailable = 100;

		private readonly IEntryRepository entryRepository;
		private readonly IPeriodRepository periodRepository;
		private readonly IEntryMapper entryMapper;
		private readonly IUnitOfWork unitOfWork;
		private readonly ITimeService timeService;
		private readonly IValidator<EntryDto> entryDtoValidator;
		private readonly IApplicationAuthenticationService applicationAuthenticationService;

		public EntryFacade(
			IEntryRepository entryRepository,
			IPeriodRepository periodRepository,
			IEntryMapper entryMapper,
			IUnitOfWork unitOfWork,
			ITimeService timeService,
			IValidator<EntryDto> entryDtoValidator,
			IApplicationAuthenticationService applicationAuthenticationService)
		{
			this.entryRepository = entryRepository;
			this.periodRepository = periodRepository;
			this.entryMapper = entryMapper;
			this.unitOfWork = unitOfWork;
			this.timeService = timeService;
			this.entryDtoValidator = entryDtoValidator;
			this.applicationAuthenticationService = applicationAuthenticationService;
		}

		public async Task<List<EntryDto>> GetMyEntriesAsync(Dto<int> periodId, CancellationToken cancellationToken = default)
		{
			var currentEmployee = await applicationAuthenticationService.GetCurrentEmployeeAsync(cancellationToken);
			var entries = await entryRepository.GetEntriesCreatedByAsync(periodId.Value, currentEmployee.Id, cancellationToken);

			return entries.Select(e => entryMapper.MapToEntryDto(e)).ToList();
		}

		public async Task<List<EntryDto>> GetReceivedEntriesAsync(Dto<int> periodId, CancellationToken cancellationToken = default)
		{
			var currentEmployee = await applicationAuthenticationService.GetCurrentEmployeeAsync(cancellationToken);
			var entries = await entryRepository.GetEntriesReceivedByAsync(periodId.Value, currentEmployee.Id, cancellationToken);

			var entriesDto = entries.Select(e => entryMapper.MapToEntryDto(e)).ToList();

			// Hide creators of the entries.
			foreach (var entry in entriesDto)
			{
				entry.CreatedById = -1;
			}

			return entriesDto;
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
			entryDtoValidator.ValidateAndThrow(newEntryDto);

			var currentEmployee = await applicationAuthenticationService.GetCurrentEmployeeAsync(cancellationToken);

			var pointsAssigned = await entryRepository.GetPointsAssignedSumAsync(newEntryDto.PeriodId, currentEmployee.Id, cancellationToken);
			int maxPoints = PointsAvailable - pointsAssigned;
			if (newEntryDto.Value > maxPoints)
			{
				throw new OperationFailedException($"Maximální počet přidělitelných bodů překročen. K dispozici zbývá {maxPoints} bodů.");
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

		public async Task UpdateEntryAsync(EntryDto entryDto, CancellationToken cancellationToken = default)
		{
			Contract.Requires<ArgumentNullException>(entryDto is not null, nameof(entryDto));
			Contract.Requires<ArgumentException>(entryDto.Id != default, nameof(entryDto.Id));
			entryDtoValidator.ValidateAndThrow(entryDto);

			var currentEmployee = await applicationAuthenticationService.GetCurrentEmployeeAsync(cancellationToken);
			var entry = await entryRepository.GetObjectAsync(entryDto.Id, cancellationToken);

			Contract.Requires<SecurityException>(entry.CreatedById == currentEmployee.Id);

			var pointsAssigned = await entryRepository.GetPointsAssignedSumAsync(entryDto.PeriodId, currentEmployee.Id, cancellationToken);
			int maxPoints = PointsAvailable - pointsAssigned + entry.Value;
			if (entryDto.Value > maxPoints)
			{
				throw new OperationFailedException($"Maximální počet přidělitelných bodů překročen. K dispozici zbývá {maxPoints} bodů.");
			}

			entryMapper.MapFromEntryDto(entryDto, entry);

			unitOfWork.AddForUpdate(entry);
			await unitOfWork.CommitAsync(cancellationToken);
		}

		public async Task<Dto<int>> GetMyRemainingPoints(Dto<int> periodId, CancellationToken cancellationToken = default)
		{
			var currentEmployee = await applicationAuthenticationService.GetCurrentEmployeeAsync(cancellationToken);

			var pointsAssigned = await entryRepository.GetPointsAssignedSumAsync(periodId.Value, currentEmployee.Id, cancellationToken);

			return Dto.FromValue(PointsAvailable - pointsAssigned);
		}

		public async Task SubmitEntriesAsync(List<int> entryIds, CancellationToken cancellationToken = default)
		{
			Contract.Requires<ArgumentNullException>(entryIds is not null, nameof(entryIds));
			Contract.Requires<ArgumentException>(entryIds.Any(), nameof(entryIds));

			var currentEmployee = await applicationAuthenticationService.GetCurrentEmployeeAsync(cancellationToken);

			var entries = await entryRepository.GetObjectsAsync(entryIds.ToArray(), cancellationToken);

			Contract.Requires<SecurityException>(entries.TrueForAll(e => e.CreatedById == currentEmployee.Id), nameof(Entry.CreatedById));

			var periodId = entries.First().PeriodId;
			Contract.Requires<OperationFailedException>(entries.TrueForAll(e => e.PeriodId == periodId), "Potvrzované záznamy musí být ze stejného období.");

			var pointsAssigned = await entryRepository.GetPointsAssignedSumAsync(periodId, currentEmployee.Id, cancellationToken);
			Contract.Requires<OperationFailedException>(pointsAssigned <= PointsAvailable, "Limit celkového počtu bodů za období překročen, zkontrolujte záznamy.");

			Contract.Requires<OperationFailedException>(entries.TrueForAll(e => e.RecipientId != currentEmployee.Id), "Nelze potrvrdit záznam, který přiřazuje body aktuálnímu uživateli.");
			Contract.Requires<OperationFailedException>(entries.TrueForAll(e => e.RecipientId != e.CreatedById), "Nelze potrvrdit záznam, který přiřazuje body sám sobě.");
			Contract.Requires<OperationFailedException>(entries.TrueForAll(e => e.Value >= 0), "Nelze potrvrdit záznam, který má záporné body.");

			foreach (var entry in entries)
			{
				entry.Submitted = timeService.GetCurrentTime();
				unitOfWork.AddForUpdate(entry);
			}

			await unitOfWork.CommitAsync();
		}

		public async Task<List<ResultItemDto>> GetResultsAsync(Dto<int> periodId, CancellationToken cancellationToken = default)
		{
			var period = await periodRepository.GetObjectAsync(periodId.Value, cancellationToken);
			//if (period.EndDate > timeService.GetCurrentDate()) // (period.ClosureDate >
			//{
			//	throw new OperationFailedException("Nelze přistupovat k výsledkům neuzavřeného období.");
			//}

			return await entryRepository.GetResultsAsync(periodId.Value, cancellationToken);
		}
	}
}

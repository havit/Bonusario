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

		public async Task<List<EntryDto>> GetMyEntriesAsync(CancellationToken cancellationToken = default)
		{
			var currentEmployee = await applicationAuthenticationService.GetCurrentEmployeeAsync(cancellationToken);
			var entries = await entryRepository.GetEntriesCreatedByAsync(currentEmployee.Id, cancellationToken);

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
	}
}

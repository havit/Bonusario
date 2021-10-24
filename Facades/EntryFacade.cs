using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Havit.Bonusario.Contracts;
using Havit.Bonusario.DataLayer.Repositories;
using Havit.Bonusario.Facades.Infrastructure.Security.Authentication;
using Havit.Bonusario.Services;
using Havit.Extensions.DependencyInjection.Abstractions;

namespace Havit.Bonusario.Facades
{
	[Service]
	public class EntryFacade : IEntryFacade
	{
		private readonly IEntryRepository entryRepository;
		private readonly IEntryMapper entryMapper;
		private readonly IApplicationAuthenticationService applicationAuthenticationService;

		public EntryFacade(
			IEntryRepository entryRepository,
			IEntryMapper entryMapper,
			IApplicationAuthenticationService applicationAuthenticationService)
		{
			this.entryRepository = entryRepository;
			this.entryMapper = entryMapper;
			this.applicationAuthenticationService = applicationAuthenticationService;
		}

		public async Task<List<EntryDto>> GetMyEntries(CancellationToken cancellationToken = default)
		{
			var currentEmployee = await applicationAuthenticationService.GetCurrentEmployeeAsync(cancellationToken);
			var entries = await entryRepository.GetEntriesCreatedByAsync(currentEmployee.Id, cancellationToken);

			return entries.Select(e => entryMapper.MapToEntryDto(e)).ToList();
		}
	}
}

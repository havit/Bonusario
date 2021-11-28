using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Havit.Blazor.Components.Web.Bootstrap;
using Havit.Bonusario.Contracts;
using Microsoft.AspNetCore.Components;

namespace Havit.Bonusario.Web.Client.Components
{
	public partial class ReceivedEntriesTable
	{
		[Parameter] public int? PeriodId { get; set; }

		[Inject] protected IEntryFacade EntryFacade { get; set; }

		private List<EntryDto> entries;

		private async Task<GridDataProviderResult<EntryDto>> GetEntries(GridDataProviderRequest<EntryDto> request)
		{
			entries = await EntryFacade.GetMyReceivedEntriesAsync(Dto.FromValue(PeriodId.Value));

			return request.ApplyTo(entries);
		}
	}
}

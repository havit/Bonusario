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

		private HxGrid<EntryDto> gridComponent;
		private List<EntryDto> entries;
		private int? loadedPeriodId;

		protected override async Task OnParametersSetAsync()
		{
			if ((PeriodId != loadedPeriodId) && (gridComponent != null))
			{
				await gridComponent.RefreshDataAsync();
			}
		}

		private async Task<GridDataProviderResult<EntryDto>> GetEntries(GridDataProviderRequest<EntryDto> request)
		{
			entries = await EntryFacade.GetMyReceivedEntriesAsync(Dto.FromValue(PeriodId.Value));
			loadedPeriodId = PeriodId;

			return request.ApplyTo(entries);
		}
	}
}

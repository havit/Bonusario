﻿using Havit.Bonusario.Web.Client.DataStores;

namespace Havit.Bonusario.Web.Client.Components;

public partial class ReadOnlyEntriesTable
{
	[Parameter] public int? PeriodId { get; set; }
	[Parameter] public bool ReceivedEntries { get; set; } = true;
	[Parameter] public bool PublicEntries { get; set; } = false;

	[Inject] protected IEntryFacade EntryFacade { get; set; }
	[Inject] protected IEmployeesDataStore EmployeesDataStore { get; set; }

	private HxGrid<EntryDto> gridComponent;
	private List<EntryDto> entries;
	private int? loadedPeriodId;

	protected override async Task OnInitializedAsync()
	{
		await EmployeesDataStore.EnsureDataAsync();
	}

	protected override async Task OnParametersSetAsync()
	{
		if ((PeriodId != loadedPeriodId) && (gridComponent != null))
		{
			await gridComponent.RefreshDataAsync();
		}
	}

	private async Task<GridDataProviderResult<EntryDto>> GetEntries(GridDataProviderRequest<EntryDto> request)
	{
		if (ReceivedEntries)
		{
			var periodIdDto = Dto.FromValue(PeriodId.Value);

			if (PublicEntries)
			{
				entries = await EntryFacade.GetAllPublicReceivedEntries(periodIdDto);
			}
			else
			{
				entries = await EntryFacade.GetMyReceivedEntriesAsync(periodIdDto);
			}
		}
		else
		{
			entries = await EntryFacade.GetMyGivenEntriesAsync(Dto.FromValue(PeriodId.Value));
		}

		loadedPeriodId = PeriodId;

		return request.ApplyTo(entries);
	}
}

using Havit.Bonusario.Web.Client.DataStores;

namespace Havit.Bonusario.Web.Client.Components;

public partial class ResultsTable
{
	[Parameter] public int? PeriodId { get; set; }

	[Inject] protected IEntryFacade EntryFacade { get; set; }
	[Inject] protected IEmployeesDataStore EmployeesDataStore { get; set; }

	private List<ResultItemDto> data;
	private HxGrid<ResultItemDto> gridComponent;
	private int? loadedPeriodId;
	private decimal grandTotal;

	protected override async Task OnParametersSetAsync()
	{
		await EmployeesDataStore.EnsureDataAsync();

		if ((loadedPeriodId != PeriodId) && (gridComponent != null))
		{
			await gridComponent.RefreshDataAsync();
		}
	}

	private async Task<GridDataProviderResult<ResultItemDto>> GetDataAsync(GridDataProviderRequest<ResultItemDto> request)
	{
		try
		{
			data = await EntryFacade.GetResultsAsync(Dto.FromValue(PeriodId.Value));
			loadedPeriodId = PeriodId;
			grandTotal = data.Sum(i => i.ValueSum);
			return request.ApplyTo(data);
		}
		catch (OperationFailedException)
		{
			return new() { Data = null, TotalCount = 0 };
		}
	}
}

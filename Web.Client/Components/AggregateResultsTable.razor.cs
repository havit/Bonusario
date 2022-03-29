using Havit.Bonusario.Web.Client.DataStores;

namespace Havit.Bonusario.Web.Client.Components;

public partial class AggregateResultsTable
{
	[Parameter] public int? PeriodSetId { get; set; }

	[Inject] protected IEntryFacade EntryFacade { get; set; }
	[Inject] protected IEmployeesDataStore EmployeesDataStore { get; set; }
	[Inject] protected IPeriodSetsDataStore PeriodSetsDataStore { get; set; }

	private List<ResultItemDto> data;
	private HxGrid<ResultItemDto> gridComponent;
	private int? loadedPeriodSetId;
	private decimal grandTotal;
	private decimal budget;

	protected override async Task OnParametersSetAsync()
	{
		await EmployeesDataStore.EnsureDataAsync();
		await PeriodSetsDataStore.EnsureDataAsync();

		if ((loadedPeriodSetId != PeriodSetId) && (gridComponent != null))
		{
			await gridComponent.RefreshDataAsync();
		}
	}

	private async Task<GridDataProviderResult<ResultItemDto>> GetDataAsync(GridDataProviderRequest<ResultItemDto> request)
	{
		try
		{
			data = await EntryFacade.GetAggregateResultsAsync(Dto.FromValue(PeriodSetId.Value));
			loadedPeriodSetId = PeriodSetId;
			grandTotal = data.Sum(i => i.ValueSum);
			budget = (await PeriodSetsDataStore.GetByKeyAsync(PeriodSetId.Value)).Budget;
			return request.ApplyTo(data);
		}
		catch (OperationFailedException)
		{
			return new() { Data = null, TotalCount = 0 };
		}
	}
}

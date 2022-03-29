using Havit.Bonusario.Web.Client.DataStores;

namespace Havit.Bonusario.Web.Client.Pages;

public partial class AggregateResults
{
	[Inject] protected IPeriodSetsDataStore PeriodSetsDataStore { get; set; }

	private int? periodSetId;

	protected override async Task OnInitializedAsync()
	{
		await PeriodSetsDataStore.EnsureDataAsync();
		periodSetId ??= PeriodSetsDataStore.GetAll().FirstOrDefault()?.Id;
	}
}

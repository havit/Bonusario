using Havit.Bonusario.Web.Client.DataStores;

namespace Havit.Bonusario.Web.Client.Pages;

public partial class Results
{
	[Inject] protected IPeriodsDataStore PeriodsDataStore { get; set; }

	private int? periodId;

	protected override async Task OnInitializedAsync()
	{
		await PeriodsDataStore.EnsureDataAsync();
		periodId ??= PeriodsDataStore.GetClosed().OrderByDescending(p => p.EndDate).FirstOrDefault()?.PeriodId;
	}
}

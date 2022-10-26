using Havit.Bonusario.Web.Client.DataStores;

namespace Havit.Bonusario.Web.Client.Pages;

public partial class HomeIndex
{
	[Inject] protected IPeriodsDataStore PeriodsDataStore { get; set; }

	private int? periodId;
	private PeriodDto period;

	protected override async Task OnInitializedAsync()
	{
		await PeriodsDataStore.EnsureDataAsync();
		period ??= PeriodsDataStore.GetActiveForSubmission().OrderBy(p => p.EndDate).FirstOrDefault();
		periodId = period?.PeriodId;
	}
}

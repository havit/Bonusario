using Havit.Bonusario.Web.Client.DataStores;

namespace Havit.Bonusario.Web.Client.Pages;

public partial class ReceivedEntries
{
	[Inject] protected IPeriodsDataStore PeriodsDataStore { get; set; }

	private int? periodId;

	private bool displayTableMyEntries;
	private bool displayTablePublicEntries;

	protected override async Task OnInitializedAsync()
	{
		await PeriodsDataStore.EnsureDataAsync();
		periodId ??= PeriodsDataStore.GetClosed().OrderByDescending(p => p.EndDate).FirstOrDefault()?.PeriodId;
	}
}

using Havit.Bonusario.Contracts.Infrastructure;
using Havit.Bonusario.Web.Client.DataStores;
using Havit.Bonusario.Web.Client.Pages.Admin.Components;
using Havit.Bonusario.Web.Client.Resources;
using Havit.Bonusario.Web.Client.Resources.Pages.Admin;

namespace Havit.Bonusario.Web.Client.Pages.Admin;

public partial class AdminIndex : ComponentBase
{
	[Inject] protected IMaintenanceFacade MaintenanceFacade { get; set; }
	[Inject] protected IHxMessengerService Messenger { get; set; }
	[Inject] protected IHxMessageBoxService MessageBox { get; set; }
	[Inject] protected INavigationLocalizer NavigationLocalizer { get; set; }
	[Inject] protected IAdminIndexLocalizer AdmninIndexLocalizer { get; set; }
	[Inject] protected IEntryFacade EntryFacade { get; set; }
	[Inject] protected IPeriodsDataStore PeriodsDataStore { get; set; }
	[Inject] protected IPeriodFacade PeriodFacade { get; set; }

	private DataSeeds dataSeedsComponent;

	private async Task HandleClearCache()
	{
		if (await MessageBox.ConfirmAsync("Do you really want to clear server cache?"))
		{
			await MaintenanceFacade.ClearCache();
			Messenger.AddInformation("Server cache cleared.");
		}
	}

	private async Task ShiftPeriods()
	{
		PeriodDto newestPeriod = await GetCurrentPeriod();

		if (newestPeriod is null)
		{
			newestPeriod = await GetLastPeriod();
		}

		await SubmitEntriesInCurrentPeriod(newestPeriod.PeriodId);
		await CreateFollowingPeriod(newestPeriod);
	}

	private async Task SubmitEntriesInCurrentPeriod(int newestPeriodId)
	{
		if (!await MessageBox.ConfirmAsync("Potvrzení", "Opravdu si přejete všechny záznamy potvrdit?"))
		{
			return;
		}

		try
		{
			var entries = await EntryFacade.GetEntriesOfPeriod(Dto.FromValue(newestPeriodId));
			await EntryFacade.SubmitEntriesAsync(entries.Where(e => e.Submitted is null).Select(e => e.Id).ToList());

			Messenger.AddInformation("Submission successful", "Entries in the current period have been submitted.");
		}
		catch (OperationFailedException)
		{
			// NOOP
		}
	}

	private async Task CreateFollowingPeriod(PeriodDto newestPeriod)
	{
		if (!await MessageBox.ConfirmAsync("Potvrzení", "Opravdu si přejete vytvořit nové období pro zadávání záznamů?"))
		{
			return;
		}

		PeriodDto newPeriod = new();

		newPeriod.StartDate = newestPeriod.StartDate.AddMonths(1);
		newPeriod.EndDate = newestPeriod.EndDate.AddMonths(1);

		newPeriod.Name = $"{newPeriod.StartDate.Month}/{newPeriod.StartDate.Year}";

		await PeriodFacade.CreateNewPeriod(newPeriod);

		Messenger.AddInformation("Period created", "The following period has been created successfully.");
	}

	private async Task<PeriodDto> GetCurrentPeriod()
	{
		await PeriodsDataStore.EnsureDataAsync();
		return PeriodsDataStore.GetActiveForSubmission().OrderBy(p => p.EndDate).FirstOrDefault();
	}

	private async Task<PeriodDto> GetLastPeriod()
	{
		await PeriodsDataStore.EnsureDataAsync();
		IEnumerable<PeriodDto> periods = await PeriodsDataStore.GetAllAsync();

		return periods.MaxBy(p => p.EndDate);
	}
}

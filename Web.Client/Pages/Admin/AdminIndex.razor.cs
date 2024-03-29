﻿using Havit.Bonusario.Contracts.Infrastructure;
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
	[Inject] protected IAdministrationOperationsFacade AdministrationOperations { get; set; }

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
		await AdministrationOperations.SubmitEntriesAndOpenNewPeriod();
		Messenger.AddInformation("Záznamy byly úspěšně potvrzeny a bylo založeno nové období.");
	}
}

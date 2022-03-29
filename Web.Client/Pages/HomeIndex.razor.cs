using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.Bonusario.Web.Client.DataStores;
using Microsoft.AspNetCore.Components;

namespace Havit.Bonusario.Web.Client.Pages;

public partial class HomeIndex
{
	[Inject] protected IPeriodsDataStore PeriodsDataStore { get; set; }

	private int? periodId;

	protected override async Task OnInitializedAsync()
	{
		await PeriodsDataStore.EnsureDataAsync();
		periodId ??= PeriodsDataStore.GetActiveForSubmission().OrderBy(p => p.EndDate).FirstOrDefault()?.PeriodId;
	}
}

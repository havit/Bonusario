﻿using System.Linq;
using System.Threading.Tasks;
using Havit.Bonusario.Web.Client.DataStores;
using Microsoft.AspNetCore.Components;

namespace Havit.Bonusario.Web.Client.Pages
{
	public partial class ReceivedEntries
	{
		[Inject] protected IPeriodsDataStore PeriodsDataStore { get; set; }

		private int? periodId;
		private bool displayTable;

		protected override async Task OnInitializedAsync()
		{
			await PeriodsDataStore.EnsureDataAsync();
			periodId ??= PeriodsDataStore.GetClosed().OrderByDescending(p => p.EndDate).FirstOrDefault()?.PeriodId;
		}
	}
}

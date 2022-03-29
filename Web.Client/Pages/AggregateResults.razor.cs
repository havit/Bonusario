using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.Bonusario.Web.Client.DataStores;
using Microsoft.AspNetCore.Components;

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

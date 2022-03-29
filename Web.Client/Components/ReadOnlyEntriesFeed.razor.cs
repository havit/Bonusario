using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Havit.Bonusario.Contracts;
using Microsoft.AspNetCore.Components;

namespace Havit.Bonusario.Web.Client.Components;

public partial class ReadOnlyEntriesFeed
{
	[Parameter] public int? PeriodId { get; set; }
	[Parameter] public bool ReceivedEntries { get; set; } = true;

	[Inject] protected IEntryFacade EntryFacade { get; set; }

	private List<EntryDto> entries;

	protected override async Task OnParametersSetAsync()
	{
		await LoadData();
	}

	private async Task LoadData()
	{
		Console.WriteLine("ReadOnlyEntriesFeed:" + PeriodId);
		if (PeriodId != null)
		{
			List<EntryDto> result = null;

			if (ReceivedEntries)
			{
				result = await EntryFacade.GetMyReceivedEntriesAsync(Dto.FromValue(PeriodId.Value));
			}
			else
			{
				result = await EntryFacade.GetMyGivenEntriesAsync(Dto.FromValue(PeriodId.Value));
			}

			entries = result.OrderByDescending(e => e.Created).ToList();
		}
	}
}

﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Havit.Bonusario.Contracts;
using Microsoft.AspNetCore.Components;

namespace Havit.Bonusario.Web.Client.Components
{
	public partial class ReceivedEntriesFeed
	{
		[Parameter] public int? PeriodId { get; set; }

		[Inject] protected IEntryFacade EntryFacade { get; set; }

		private List<EntryDto> entries;

		protected override async Task OnParametersSetAsync()
		{
			await LoadData();
		}

		private async Task LoadData()
		{
			var result = await EntryFacade.GetReceivedEntriesAsync(Dto.FromValue(PeriodId.Value));
			entries = result.OrderByDescending(e => e.Created).ToList();
		}
	}
}

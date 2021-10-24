using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.Bonusario.Contracts;
using Havit.Bonusario.Web.Client.DataStores;
using Microsoft.AspNetCore.Components;

namespace Havit.Bonusario.Web.Client.Pages.Entries
{
	public partial class MyEntriesFeed
	{
		[Parameter] public int? PeriodId { get; set; }

		[Inject] protected IEntryFacade EntryFacade { get; set; }

		private EntryDto newEntry = new EntryDto();
		private List<EntryDto> entries;

		protected override async Task OnParametersSetAsync()
		{
			newEntry.PeriodId = this.PeriodId.Value;
			await LoadEntries();
		}

		private async Task LoadEntries()
		{
			var data = await EntryFacade.GetMyEntriesAsync(Dto.FromValue(PeriodId.Value));
			entries = data.OrderByDescending(e => e.Created).ToList();
		}

		private async Task HandleEntryDeleted()
		{
			await LoadEntries();
		}

		private async Task HandleEntryCreated()
		{
			newEntry = new EntryDto() { PeriodId = PeriodId.Value };
			await LoadEntries();
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.Bonusario.Contracts;
using Havit.Bonusario.Web.Client.DataStores;
using Microsoft.AspNetCore.Components;

namespace Havit.Bonusario.Web.Client.Components
{
	public partial class MyEntriesFeed
	{
		[Parameter] public int? PeriodId { get; set; }

		[Inject] protected IEntryFacade EntryFacade { get; set; }

		private EntryDto newEntry = new EntryDto();
		private List<EntryDto> entries;
		private int? remainingPoints;

		protected override async Task OnParametersSetAsync()
		{
			newEntry.PeriodId = this.PeriodId.Value;
			await LoadData();
		}

		private async Task LoadData()
		{
			var result = await EntryFacade.GetMyEntriesAsync(Dto.FromValue(PeriodId.Value));
			entries = result.OrderByDescending(e => e.Created).ToList();

			remainingPoints = (await EntryFacade.GetMyRemainingPoints(Dto.FromValue(PeriodId.Value))).Value;
		}

		private async Task HandleEntryDeleted()
		{
			await LoadData();
		}

		private async Task HandleEntryUpdated()
		{
			await LoadData();
		}

		private async Task HandleEntryCreated()
		{
			newEntry = new EntryDto() { PeriodId = PeriodId.Value };
			await LoadData();
		}
	}
}

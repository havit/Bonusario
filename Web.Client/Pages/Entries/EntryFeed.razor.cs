using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.Bonusario.Contracts;
using Microsoft.AspNetCore.Components;

namespace Havit.Bonusario.Web.Client.Pages.Entries
{
	public partial class EntryFeed
	{
		[Inject] protected IEntryFacade EntryFacade { get; set; }

		private List<EntryDto> entries;

		protected override async Task OnInitializedAsync()
		{
			entries ??= await LoadEntries();
			StateHasChanged();
		}

		private async Task<List<EntryDto>> LoadEntries()
		{
			return await EntryFacade.GetMyEntries();
		}
	}
}

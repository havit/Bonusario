using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Havit.Blazor.Components.Web.Bootstrap;
using Havit.Bonusario.Contracts;
using Havit.Bonusario.Web.Client.DataStores;
using Havit.Diagnostics.Contracts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Havit.Bonusario.Web.Client.Components
{
	public partial class EntryCard
	{
		[Parameter] public EntryDto Entry { get; set; }
		[Parameter] public EventCallback OnEntryDeleted { get; set; }
		[Parameter] public EventCallback<EntryDto> OnEntryCreated { get; set; }
		[Parameter] public EventCallback<EntryDto> OnEntryUpdated { get; set; }

		[Inject] protected IEmployeesDataStore EmployeesDataStore { get; set; }
		[Inject] protected IEntryFacade EntryFacade { get; set; }

		private EditContext editContext;

		protected override void OnParametersSet()
		{
			editContext = new EditContext(Entry);
			editContext.OnFieldChanged += EditContext_OnFieldChanged;
		}

		private void EditContext_OnFieldChanged(object sender, FieldChangedEventArgs e)
		{
			if (editContext.Validate())
			{
				InvokeAsync(async () =>
				{
					try
					{
						await EntryFacade.UpdateEntryAsync(this.Entry);
						await OnEntryUpdated.InvokeAsync(this.Entry);
					}
					catch (OperationFailedException)
					{
						// NOOP
					}
				});
			}
		}

		protected override async Task OnInitializedAsync()
		{
			await EmployeesDataStore.EnsureDataAsync();
		}

		private async Task HandleDeleteClick()
		{
			Contract.Assert(Entry.Submitted is null, "Nelze smazat odeslaný záznam.");
			await EntryFacade.DeleteEntryAsync(Dto.FromValue(Entry.Id));
			await OnEntryDeleted.InvokeAsync();
		}

		private async Task HandleNewClick()
		{
			Contract.Assert(Entry.Id == default, "Záznam již není nový.");
			Contract.Assert(Entry.PeriodId != default, "PeriodId musí být nastaven.");

			try
			{
				this.Entry.Id = (await EntryFacade.CreateEntryAsync(this.Entry)).Value;
				await OnEntryCreated.InvokeAsync(this.Entry);
			}
			catch (OperationFailedException)
			{
				// NOOP
			}
		}

		private List<string> preconfiguredTags = new List<string>()
		{
			"důvěra",
			"férovost",
			"nezávislost",
			"potěšení",
			"přátelskost",
			"radost",
			"stabilita",
			"team",
			"vášeň",
			"vzdělávání",
			"znalosti",
		};
		private Task<InputTagsDataProviderResult> GetTagSuggestions(InputTagsDataProviderRequest request)
		{
			return Task.FromResult(new InputTagsDataProviderResult()
			{
				Data = preconfiguredTags.Select(v => v.ToString()).Where(v => !this.Entry.Tags.Contains(v))
			});
		}
	}
}

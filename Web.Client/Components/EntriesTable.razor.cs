using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Havit.Blazor.Components.Web;
using Havit.Bonusario.Contracts;
using Havit.Bonusario.Web.Client.DataStores;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;

namespace Havit.Bonusario.Web.Client.Components
{
	public partial class EntriesTable
	{
		[Parameter] public int? PeriodId { get; set; }

		[Inject] protected IEmployeesDataStore EmployeesDataStore { get; set; }
		[Inject] protected IEntryFacade EntryFacade { get; set; }
		[Inject] protected IHxMessageBoxService MessageBox { get; set; }
		[Inject] protected AuthenticationStateProvider AuthenticationStateProvider { get; set; }

		private IEnumerable<EmployeeReferenceDto> employees;
		private List<EntryDto> entries;
		private NewEntriesModel newEntriesModel;
		private EditContext newEntriesEditContext;
		private int? remainingPoints;

		protected override async Task OnInitializedAsync()
		{
			employees ??= await EmployeesDataStore.GetAllAsync();
			await LoadData();
		}

		protected override async Task OnParametersSetAsync()
		{
			await LoadData();
		}

		private async Task LoadData()
		{
			if (PeriodId != null)
			{
				var e = await EntryFacade.GetMyEntriesAsync(Dto.FromValue(PeriodId.Value));
				entries = e.OrderByDescending(e => e.Created).ToList();

				remainingPoints = (await EntryFacade.GetMyRemainingPoints(Dto.FromValue(PeriodId.Value))).Value;

				var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
				var currentUserEmail = authState.User.FindFirst("preferred_username").Value;

				newEntriesModel = new()
				{
					Entries = employees.Where(e => !String.Equals(e.Email, currentUserEmail, StringComparison.OrdinalIgnoreCase)).Select(employee => new EntryDto()
					{
						RecipientId = employee.EmployeeId,
						PeriodId = this.PeriodId.Value
					}).ToList()
				};
				newEntriesEditContext = new EditContext(newEntriesModel);
			}
		}

		private string GetBadgePopoverTitle(EntryDto entry)
		{
			return $"₿ {entry.Value}";
		}

		private string GetBadgePopoverContent(EntryDto entry)
		{
			var result = $@"<div class=""d-5"">{(String.IsNullOrWhiteSpace(entry.Text) ? "<i>bez textu</i>" : entry.Text)}</div>";
			if (entry.Tags.Any())
			{
				var tags = entry.Tags.Aggregate<string, string>(String.Empty, (acc, tag) => acc + $"<span class=\"badge bg-light text-dark me-2\">{tag}</span>");
				result = result + "<div class=\"mt-2\">" + tags + "</div>";
			}
			return result;
		}

		private async Task HandleSubmitAllClick()
		{
			if (await MessageBox.ConfirmAsync("Potvrzení", "Opravdu si přejete všechny záznamy vložit a potvrdit?"))
			{
				try
				{
					foreach (var entry in newEntriesModel.Entries)
					{
						if (entry.HasValues())
						{
							entry.Id = (await EntryFacade.CreateEntryAsync(entry)).Value;
						}
					}
					await LoadData(); // reload new entries
					await EntryFacade.SubmitEntriesAsync(entries.Where(e => e.Submitted is null).Select(e => e.Id).ToList());
					await LoadData();
				}
				catch (OperationFailedException)
				{
					// NOOP
				}
			}
		}

		public class NewEntriesModel
		{
			public List<EntryDto> Entries { get; set; }

			public int TotalPointsAssigned => Entries.Sum(e => e.Value);

			public class Validator : AbstractValidator<NewEntriesModel>
			{
				public Validator(IValidator<EntryDto> entryDtoValidator)
				{
					RuleForEach(i => i.Entries).SetValidator(entryDtoValidator);
				}
			}
		}
	}
}

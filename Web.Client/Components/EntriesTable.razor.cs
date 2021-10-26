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
using Microsoft.AspNetCore.Components.Forms;

namespace Havit.Bonusario.Web.Client.Components
{
	public partial class EntriesTable : IDisposable
	{
		[Parameter] public int PeriodId { get; set; }

		[Inject] protected IEmployeesDataStore EmployeesDataStore { get; set; }
		[Inject] protected IEntryFacade EntryFacade { get; set; }
		[Inject] protected IHxMessageBoxService MessageBox { get; set; }

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
			var e = await EntryFacade.GetMyEntriesAsync(Dto.FromValue(PeriodId));
			entries = e.OrderByDescending(e => e.Created).ToList();

			remainingPoints = (await EntryFacade.GetMyRemainingPoints(Dto.FromValue(PeriodId))).Value;

			newEntriesModel = new()
			{
				Entries = employees.Select(employee => new EntryDto()
				{
					RecipientId = employee.EmployeeId
				}).ToList()
			};
			if (newEntriesEditContext is not null)
			{
				newEntriesEditContext.OnFieldChanged -= NewEntriesEditContext_OnFieldChanged;
			}
			newEntriesEditContext = new EditContext(newEntriesModel);
			newEntriesEditContext.OnFieldChanged += NewEntriesEditContext_OnFieldChanged;
		}

		private void NewEntriesEditContext_OnFieldChanged(object sender, FieldChangedEventArgs e)
		{
			// throw new NotImplementedException();
		}

		private async Task HandleEntryDeleted() => await LoadData();
		private async Task HandleEntryUpdated() => await LoadData();
		private async Task HandleEntryCreated() => await LoadData();

		private async Task HandleSubmitAllClick()
		{
			if (await MessageBox.ConfirmAsync("Potvrzení", "Opravdu si přejete všechny koncepty potvrdit?"))
			{
				try
				{
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

		public void Dispose()
		{
			if (newEntriesEditContext is not null)
			{
				newEntriesEditContext.OnFieldChanged -= NewEntriesEditContext_OnFieldChanged;
			}
		}
	}
}

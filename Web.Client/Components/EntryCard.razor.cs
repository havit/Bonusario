using System.Text;
using Havit.Bonusario.Web.Client.DataStores;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;

namespace Havit.Bonusario.Web.Client.Components;

public partial class EntryCard
{
	[Parameter] public EntryDto Entry { get; set; }
	[Parameter] public bool RecipientLocked { get; set; } = false;
	[Parameter] public bool ShowAuthor { get; set; } = false;
	[Parameter] public EventCallback OnEntryDeleted { get; set; }
	[Parameter] public EventCallback<EntryDto> OnEntryCreated { get; set; }
	[Parameter] public EventCallback<EntryDto> OnEntryUpdated { get; set; }
	[Parameter] public EventCallback OnCloseButtonClicked { get; set; }
	[Parameter] public string CssClass { get; set; }

	[Inject] protected IEmployeeFacade EmployeeFacade { get; set; }
	[Inject] protected IEmployeesDataStore EmployeesDataStore { get; set; }
	[Inject] protected IEntryFacade EntryFacade { get; set; }

	private EditContext editContext;

	private bool RenderAuthor => ShowAuthor && Entry.CreatedById.HasValue;
	/// <summary>
	/// Indicates whether the entry is being edited or created.
	/// If <c>true</c>, it is being edited. If <c>false</c> it is a new entry.
	/// </summary>
	private bool EdittingEntry => Entry.Id != default;
	private bool RenderCloseButton => OnCloseButtonClicked.HasDelegate;

	protected override void OnInitialized()
	{
		editContext = new(new EntryDto());
	}

	protected override void OnParametersSet()
	{
		editContext = new EditContext(Entry);
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

	private async Task HandleCreateOrUpdateButtonClick()
	{
		if (Entry.Id == default && Entry.Submitted is null)
		{
			await CreateNewEntry();
		}
		else
		{
			await UpdateEntry();
		}
	}

	private async Task CreateNewEntry()
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

	private async Task UpdateEntry()
	{
		if ((Entry.Id == default) || (Entry.Submitted is not null))
		{
			return;
		}

		if (editContext.Validate())
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
		}
	}

	private async Task HandleCloseButtonClick()
	{
		await OnCloseButtonClicked.InvokeAsync();
	}
}

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
	[Parameter] public bool SettingDefaultAuthorIdentityVisibilityEnabled { get; set; }

	[Inject] protected IEmployeeFacade EmployeeFacade { get; set; }
	[Inject] protected IEmployeesDataStore EmployeesDataStore { get; set; }
	[Inject] protected IEntryFacade EntryFacade { get; set; }

	private EditContext editContext;

	private AuthorIdentityVisibility? defaultAuthorIdentityVisibility = null;

	private bool RenderAuthor => ShowAuthor && Entry.CreatedById.HasValue;
	private bool DefaultSelected => Entry.AuthorIdentityVisibility == defaultAuthorIdentityVisibility;

	private bool parametersHaveBeenSet = false;

	/// <summary>
	/// Popover hinting that the user can set default <c>AuthorIdentityVisibility</c> by clicking wrapped button.
	/// </summary>
	private HxPopover popover;

	protected override void OnInitialized()
	{
		editContext = new(new EntryDto());
	}

	protected override async Task OnParametersSetAsync()
	{
		editContext = new EditContext(Entry);
		editContext.OnFieldChanged += EditContext_OnFieldChanged;

		if (!parametersHaveBeenSet)
		{
			Entry.AuthorIdentityVisibility = await GetDefaultIdentityVisibility();
		}

		parametersHaveBeenSet = true;
	}

	private void EditContext_OnFieldChanged(object sender, FieldChangedEventArgs e)
	{
		if ((Entry.Id == default) || (Entry.Submitted is not null))
		{
			return;
		}

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

		if (SettingDefaultAuthorIdentityVisibilityEnabled)
		{
			await GetDefaultIdentityVisibility();
		}
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

	public async Task<AuthorIdentityVisibility> GetDefaultIdentityVisibility()
	{
		defaultAuthorIdentityVisibility ??= (await EmployeeFacade.GetCurrentEmployeeDefaultIdentityVisibility()).Value;
		return defaultAuthorIdentityVisibility.GetValueOrDefault();
	}

	private async Task SetDefaultAuthorIdentityVisibility()
	{
		await popover.HideAsync();

		defaultAuthorIdentityVisibility = Entry.AuthorIdentityVisibility;
		await EmployeeFacade.UpdateCurrentEmployeeDefaultIdentityVisibility(Dto.FromValue(await GetDefaultIdentityVisibility()));
	}
}

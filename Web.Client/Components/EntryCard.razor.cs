using System.Text;
using Havit.Bonusario.Web.Client.DataStores;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;

namespace Havit.Bonusario.Web.Client.Components;

public partial class EntryCard
{
	private static readonly Dictionary<EntryVisibility, BootstrapIcon> EntryVisibilityIcons = new()
	{
		{ EntryVisibility.RecipientOnlyAnonymous, BootstrapIcon.PersonXFill },
		{ EntryVisibility.RecipientOnlyWithAuthorIdentity, BootstrapIcon.PersonHeart },
		{ EntryVisibility.Public, BootstrapIcon.Globe }
	};

	[Parameter] public EntryDto Entry { get; set; }
	[Parameter] public bool RecipientLocked { get; set; } = false;
	[Parameter] public bool ShowAuthor { get; set; } = false;
	[Parameter] public EventCallback OnEntryDeleted { get; set; }
	[Parameter] public EventCallback<EntryDto> OnEntryCreated { get; set; }
	[Parameter] public EventCallback<EntryDto> OnEntryUpdated { get; set; }

	[Inject] protected IEmployeeFacade EmployeeFacade { get; set; }
	[Inject] protected IEmployeesDataStore EmployeesDataStore { get; set; }
	[Inject] protected IEntryFacade EntryFacade { get; set; }

	private EditContext editContext;

	private bool RenderAuthor => ShowAuthor && Entry.CreatedById.HasValue;

	protected override void OnInitialized()
	{
		editContext = new(new EntryDto());
	}

	protected override void OnParametersSet()
	{
		editContext = new EditContext(Entry);
		editContext.OnFieldChanged += EditContext_OnFieldChanged;
	}

	private void EditContext_OnFieldChanged(object sender, FieldChangedEventArgs e)
	{
		HandleFieldChanged();
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

	private void ChangeEntryVisibility(EntryVisibility visibility)
	{
		Entry.Visibility = visibility;
		HandleFieldChanged();
	}

	private string GetEntryVisibilityText(EntryVisibility entryVisibility)
	{
		StringBuilder visibilityText = new(EnumExt.GetDescription(typeof(EntryVisibility), entryVisibility));
		visibilityText[0] = char.ToUpper(visibilityText[0]);

		return visibilityText.ToString();
	}

	private void HandleFieldChanged()
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

	private BootstrapIcon GetIconForEntryVisibility(EntryVisibility visibility)
	{
		bool success = EntryVisibilityIcons.TryGetValue(visibility, out BootstrapIcon icon);

		if (success)
		{
			return icon;
		}
		else
		{
			throw new InvalidOperationException("An icon has to be specified for all entry visibilities.");
		}
	}
}

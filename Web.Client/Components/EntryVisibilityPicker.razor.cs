using System.Text;

namespace Havit.Bonusario.Web.Client.Components;

public partial class EntryVisibilityPicker
{
	private static readonly Dictionary<EntryVisibility, BootstrapIcon> EntryVisibilityIcons = new()
	{
		{ EntryVisibility.RecipientOnlyAnonymous, BootstrapIcon.Incognito },
		{ EntryVisibility.RecipientOnlyWithAuthorIdentity, BootstrapIcon.PersonHeart },
		{ EntryVisibility.Public, BootstrapIcon.Globe }
	};

	[Parameter] public EntryVisibility Value { get; set; }
	[Parameter] public EventCallback<EntryVisibility> ValueChanged { get; set; }

	[Parameter] public bool Readonly { get; set; }

	[Parameter] public string CssClass { get; set; }

	private async Task HandleEntryVisibilityChanged(EntryVisibility entryVisibility)
	{
		if (entryVisibility != Value)
		{
			Value = entryVisibility;
			await ValueChanged.InvokeAsync(entryVisibility);
		}
	}

	private string GetEntryVisibilityText(EntryVisibility entryVisibility)
	{
		return EnumExt.GetDescription(typeof(EntryVisibility), entryVisibility);
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

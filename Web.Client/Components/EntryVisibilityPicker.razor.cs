using System.Text;

namespace Havit.Bonusario.Web.Client.Components;

public partial class EntryVisibilityPicker
{
	private const string publicEntryText = "Viditelné všem";
	private const string visibleOnlyToRecipientText = "Viditelné příjemci";

	[Parameter] public bool Value { get; set; }
	[Parameter] public EventCallback<bool> ValueChanged { get; set; }

	[Parameter] public bool Readonly { get; set; }

	[Parameter] public string CssClass { get; set; }

	private bool[] possibleValues = { true, false };

	private async Task HandleEntryVisibilityChanged(bool entryPublic)
	{
		if (entryPublic != Value)
		{
			Value = entryPublic;
			await ValueChanged.InvokeAsync(entryPublic);
		}
	}

	private string GetTextForValue(bool entryPublic)
	{
		return entryPublic ? publicEntryText : visibleOnlyToRecipientText;
	}

	private BootstrapIcon GetIconForValue(bool entryPublic)
	{
		return entryPublic ? BootstrapIcon.Globe : BootstrapIcon.PersonHeart;
	}
}

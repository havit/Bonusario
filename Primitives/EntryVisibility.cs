using System.ComponentModel;

namespace Havit.Bonusario.Primitives;

public enum EntryVisibility
{
	[Description("Pro příjemce bez podpisu")]
	RecipientOnlyAnonymous = 0,

	[Description("Pro příjemce s podpisem")]
	RecipientOnlyWithAuthorIdentity = 1,

	[Description("Viditelné všem")]
	Public = 2
}

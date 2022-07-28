using System.ComponentModel;

namespace Havit.Bonusario.Primitives;

public enum EntryVisibility
{
	[Description("pro příjemce bez podpisu")]
	RecipientOnlyAnonymous = 0,

	[Description("pro příjemce s podpisem")]
	RecipientOnlyWithAuthorIdentity = 1,

	[Description("viditelné všem")]
	Public = 2
}

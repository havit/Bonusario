using System.ComponentModel;

namespace Havit.Bonusario.Primitives;

public enum AuthorIdentityVisibility
{
	[Description("pro příjemce bez podpisu")]
	Hidden = 0,

	[Description("pro příjemce s podpisem")]
	VisibleOnlyForRecipient = 1,

	[Description("viditelné všem")]
	Public = 2
}

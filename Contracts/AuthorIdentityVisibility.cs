using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Bonusario.Contracts;

public enum AuthorIdentityVisibility
{
	[Description("pro příjemce bez podpisu")]
	Hidden,

	[Description("pro příjemce s podpisem")]
	VisibleOnlyForRecipient,

	[Description("viditelné všem")]
	Public
}

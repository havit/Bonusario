using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Bonusario.Contracts;

public enum AuthorIdentityVisibility
{
	[Description("Anonymní")]
	Hidden,

	[Description("Veřejné pro příjemce")]
	VisibleOnlyForRecipient,

	[Description("Veřejné")]
	Public
}

using System;
using Microsoft.AspNetCore.Components;

namespace Havit.Bonusario.Web.Client.Components
{
	public partial class RemainingPoints
	{
		[Parameter] public int Points { get; set; }
	}
}

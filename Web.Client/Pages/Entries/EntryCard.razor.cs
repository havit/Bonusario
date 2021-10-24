using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.Bonusario.Contracts;
using Havit.Bonusario.Web.Client.DataStores;
using Microsoft.AspNetCore.Components;

namespace Havit.Bonusario.Web.Client.Pages.Entries
{
	public partial class EntryCard
	{
		[Parameter] public EntryDto Entry { get; set; }

		[Inject] protected IEmployeesDataStore EmployeesDataStore { get; set; }


		protected override async Task OnInitializedAsync()
		{
			await EmployeesDataStore.EnsureDataAsync();
		}
	}
}

using Havit.Bonusario.Web.Client.DataStores;

namespace Havit.Bonusario.Web.Client.Components;

public partial class ReadonlyEntryCard
{
	[Parameter] public EntryDto Entry { get; set; }
	[Parameter] public bool ShowAuthor { get; set; }

	[Inject] protected IEmployeesDataStore EmployeesDataStore { get; set; }

	protected override async Task OnInitializedAsync()
	{
		await EmployeesDataStore.EnsureDataAsync();
	}
}

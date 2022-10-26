using Havit.Bonusario.Web.Client.DataStores;

namespace Havit.Bonusario.Web.Client.Components;

public partial class ReadonlyEntryCard
{
	[Parameter] public EntryDto Entry { get; set; }
	[Parameter] public bool ShowAuthor { get; set; }
	[Parameter] public EventCallback OnClick { get; set; }
	[Parameter] public string CssClass { get; set; }

	[Inject] protected IEmployeesDataStore EmployeesDataStore { get; set; }

	protected bool HasTags => Entry.Tags is not null && Entry.Tags.Count > 0;
	protected bool HasText => !string.IsNullOrWhiteSpace(Entry.Text);

	protected override async Task OnInitializedAsync()
	{
		await EmployeesDataStore.EnsureDataAsync();
	}

	private async Task HandleClick()
	{
		await OnClick.InvokeAsync();
	}
}

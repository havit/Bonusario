using Microsoft.JSInterop;

namespace Havit.Bonusario.Web.Client.Components;

public partial class MyEntriesFeed
{
	[Parameter] public int? PeriodId { get; set; }

	[Inject] protected IEntryFacade EntryFacade { get; set; }
	[Inject] protected IEmployeeFacade EmployeeFacade { get; set; }

	[Inject] protected IJSRuntime JSRuntime { get; set; }

	private IJSObjectReference jsModule;
	private DotNetObjectReference<MyEntriesFeed> dotnetObjectReference;
	private string entryCardFormWrapperId = "card" + Guid.NewGuid().ToString("N");
	private EntryDto editedEntry = new();
	private List<EntryDto> entries;
	private int? remainingPoints;

	public MyEntriesFeed()
	{
		dotnetObjectReference = DotNetObjectReference.Create(this);
	}

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			await EnsureJsModule();
			await jsModule.InvokeVoidAsync("initialize", dotnetObjectReference, entryCardFormWrapperId);
		}
	}

	protected async Task EnsureJsModule()
	{
		jsModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", "/js/MyEntriesFeed.js");
	}

	protected override async Task OnParametersSetAsync()
	{
		editedEntry.PeriodId = this.PeriodId.Value;
		await LoadData();
	}

	private async Task LoadData()
	{
		var result = await EntryFacade.GetMyGivenEntriesAsync(Dto.FromValue(PeriodId.Value));
		entries = result.OrderByDescending(e => e.Created).ToList();

		remainingPoints = (await EntryFacade.GetMyRemainingPoints(Dto.FromValue(PeriodId.Value))).Value;
	}

	private async Task EditEntry(EntryDto entry)
	{
		editedEntry = entry;
		await JSRuntime.InvokeVoidAsync("window.scrollTo", 0, 0);
	}

	[JSInvokable("HandleBodyClick")]
	public void HandleBodyClick()
	{
		EditNewEntry();
	}

	private void EditNewEntry()
	{
		if (editedEntry.Id != default)
		{
			editedEntry = new();
			StateHasChanged();
		}
	}

	private async Task HandleEntryDeleted()
	{
		EditNewEntry();
		await LoadData();
	}

	private async Task HandleEntryUpdated()
	{
		await LoadData();
	}

	private async Task HandleEntryCreated()
	{
		editedEntry = new EntryDto() { PeriodId = PeriodId.Value, Visibility = EntryDto.DefaultVisibility };
		await LoadData();
	}
}

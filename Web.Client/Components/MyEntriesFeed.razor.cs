namespace Havit.Bonusario.Web.Client.Components;

public partial class MyEntriesFeed
{
	[Parameter] public int? PeriodId { get; set; }

	[Inject] protected IEntryFacade EntryFacade { get; set; }
	[Inject] protected IEmployeeFacade EmployeeFacade { get; set; }

	private EntryDto newEntry = new();
	private EntryDto editedEntry;

	private List<EntryDto> entries;
	private int? remainingPoints;
	private GivenPointsSummary givenPointsSummary;

	private bool givenPointsSummaryCollapseExpanded;

	protected override async Task OnParametersSetAsync()
	{
		newEntry.PeriodId = PeriodId.Value;
		await LoadData();
	}

	private async Task HandleEntryUpdatedOrDeleted()
	{
		await LoadData();
		CloseEdit();
	}

	private void HandleGivenPointsSummaryCollapseStateChanged(bool expanded)
	{
		givenPointsSummaryCollapseExpanded = expanded;
	}

	private void EditEntry(EntryDto entry)
	{
		editedEntry = entry;
	}

	private void CloseEdit()
	{
		editedEntry = null;
	}

	private async Task HandleEntryCreated()
	{
		EditNewEntry();
		await LoadData();
	}

	private async Task LoadData()
	{
		var result = await EntryFacade.GetMyGivenEntriesAsync(Dto.FromValue(PeriodId.Value));
		entries = result.OrderByDescending(e => e.Created).ToList();

		remainingPoints = (await EntryFacade.GetMyRemainingPoints(Dto.FromValue(PeriodId.Value))).Value;

		await givenPointsSummary.ReloadData();
	}

	private void EditNewEntry()
	{
		newEntry = new EntryDto() { PeriodId = PeriodId.Value, Visibility = EntryDto.DefaultVisibility };
	}
}

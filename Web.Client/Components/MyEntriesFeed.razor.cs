namespace Havit.Bonusario.Web.Client.Components;

public partial class MyEntriesFeed
{
	[Parameter] public int? PeriodId { get; set; }

	[Inject] protected IEntryFacade EntryFacade { get; set; }
	[Inject] protected IEmployeeFacade EmployeeFacade { get; set; }

	private EntryDto newEntry = new();
	private List<EntryDto> entries;
	private int? remainingPoints;
	/// <summary>
	/// EntryCard for creating new entries.
	/// </summary>
	private EntryCard entryCard;

	private int? editedEntryId;

	protected override async Task OnParametersSetAsync()
	{
		newEntry.PeriodId = this.PeriodId.Value;
		await LoadData();
	}

	private async Task LoadData()
	{
		var result = await EntryFacade.GetMyGivenEntriesAsync(Dto.FromValue(PeriodId.Value));
		entries = result.OrderByDescending(e => e.Created).ToList();

		remainingPoints = (await EntryFacade.GetMyRemainingPoints(Dto.FromValue(PeriodId.Value))).Value;
	}

	private async Task HandleEntryDeleted()
	{
		await LoadData();
	}

	private async Task HandleEntryUpdated()
	{
		await LoadData();
	}

	private async Task HandleEntryCreated()
	{
		newEntry = new EntryDto() { PeriodId = PeriodId.Value, Visibility = await entryCard.GetDefaultEntryVisibility() };
		await LoadData();
	}
}

namespace Havit.Bonusario.Web.Client.Components;

public partial class RecipientEntries
{
	[Parameter] public int RecipientId { get; set; }
	[Parameter] public int PeriodId { get; set; }
	[Parameter] public int RemainingPoints { get; set; }
	[Parameter] public IEnumerable<EntryDto> Entries { get; set; }
	[Parameter] public EventCallback OnEntryDeleted { get; set; }
	[Parameter] public EventCallback<EntryDto> OnEntryCreated { get; set; }
	[Parameter] public EventCallback<EntryDto> OnEntryUpdated { get; set; }

	private EntryDto newEntry = new EntryDto();

	protected override void OnParametersSet()
	{
		newEntry.PeriodId = this.PeriodId;
		newEntry.RecipientId = this.RecipientId;
	}

	private async Task HandleEntryCreated(EntryDto createdEntry)
	{
		newEntry = new EntryDto() { PeriodId = PeriodId };
		await OnEntryCreated.InvokeAsync(createdEntry);
	}
}

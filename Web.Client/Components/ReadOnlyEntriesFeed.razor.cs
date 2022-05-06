namespace Havit.Bonusario.Web.Client.Components;

public partial class ReadOnlyEntriesFeed
{
	[Parameter] public int? PeriodId { get; set; }
	[Parameter] public bool ReceivedEntries { get; set; } = true;

	[Inject] protected IEntryFacade EntryFacade { get; set; }

	private List<EntryDto> entries;

	protected override async Task OnParametersSetAsync()
	{
		await LoadData();
	}

	private async Task LoadData()
	{
		Console.WriteLine("ReadOnlyEntriesFeed:" + PeriodId);
		if (PeriodId != null)
		{
			List<EntryDto> result = null;

			if (ReceivedEntries)
			{
				var periodIdDto = Dto.FromValue(PeriodId.Value);

				var myReceivedEntries = await EntryFacade.GetMyReceivedEntriesAsync(periodIdDto);
				var allPublicReceivedEntries = await EntryFacade.GetAllPublicReceivedEntries(periodIdDto);

				entries = myReceivedEntries.Concat(allPublicReceivedEntries).ToList();
			}
			else
			{
				result = await EntryFacade.GetMyGivenEntriesAsync(Dto.FromValue(PeriodId.Value));
				entries = result.OrderByDescending(e => e.Created).ToList();
			}
		}
	}
}

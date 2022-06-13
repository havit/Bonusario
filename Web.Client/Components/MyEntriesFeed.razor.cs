namespace Havit.Bonusario.Web.Client.Components;

public partial class MyEntriesFeed
{
	[Parameter] public int? PeriodId { get; set; }

	[Inject] protected IEntryFacade EntryFacade { get; set; }
	[Inject] protected IEmployeeFacade EmployeeFacade { get; set; }

	private EntryDto newEntry = new();
	private List<EntryDto> entries;
	private int? remainingPoints;

	private AuthorIdentityVisibility defaultAuthorIdentityVisibility;

	protected override async Task OnParametersSetAsync()
	{
		defaultAuthorIdentityVisibility = await GetDefaultIdentityVisibility();

		newEntry.PeriodId = this.PeriodId.Value;
		newEntry.AuthorIdentityVisibility = defaultAuthorIdentityVisibility;
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
		newEntry = new EntryDto() { PeriodId = PeriodId.Value, AuthorIdentityVisibility = await GetDefaultIdentityVisibility() };
		await LoadData();
	}

	private async Task<AuthorIdentityVisibility> GetDefaultIdentityVisibility()
	{
		return (await EmployeeFacade.GetCurrentEmployeeDefaultIdentityVisibility()).Value;
	}

	private async Task SetDefaultEntryAuthorIdentityVisibility(AuthorIdentityVisibility newDefaultIdentityVisibility)
	{
		await EmployeeFacade.UpdateCurrentEmployeeDefaultIdentityVisibility(Dto.FromValue(newDefaultIdentityVisibility));
	}
}

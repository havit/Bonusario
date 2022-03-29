using Havit.Blazor.Components.Web.Services.DataStores;

namespace Havit.Bonusario.Web.Client.DataStores;

public class PeriodsDataStore : DictionaryStaticDataStore<int, PeriodDto>, IPeriodsDataStore
{
	private readonly Func<IPeriodFacade> periodFacade;

	public PeriodsDataStore(Func<IPeriodFacade> PeriodFacade)
	{
		this.periodFacade = PeriodFacade;
	}

	protected override Func<PeriodDto, int> KeySelector { get; } = (dto) => dto.PeriodId;

	public List<PeriodDto> GetActiveForSubmission()
	{
		return GetAll()
			.Where(p => p.StartDate <= DateTime.Today)
			.Where(p => p.EndDate >= DateTime.Today)
			.ToList();
	}

	public async Task<List<PeriodDto>> GetActiveForSubmissionAsync()
	{
		await EnsureDataAsync();
		return GetActiveForSubmission();
	}

	public List<PeriodDto> GetClosed()
	{
		return GetAll()
			.Where(p => p.EndDate < DateTime.Today)
			.ToList();
	}

	public async Task<List<PeriodDto>> GetClosedAsync()
	{
		await EnsureDataAsync();
		return GetClosed();
	}

	protected override async Task<IEnumerable<PeriodDto>> LoadDataAsync()
	{
		return await periodFacade().GetAllPeriodsAsync();
	}

	protected override bool ShouldRefresh() => false;
}

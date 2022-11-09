using Havit.Blazor.Components.Web.Services.DataStores;

namespace Havit.Bonusario.Web.Client.DataStores;

public class PeriodSetsDataStore : DictionaryStaticDataStore<int, PeriodSetDto>, IPeriodSetsDataStore
{
	private readonly IPeriodSetFacade periodSetFacade;

	public PeriodSetsDataStore(IPeriodSetFacade periodSetFacade)
	{
		this.periodSetFacade = periodSetFacade;
	}

	protected override Func<PeriodSetDto, int> KeySelector { get; } = (dto) => dto.Id;

	protected override async Task<IEnumerable<PeriodSetDto>> LoadDataAsync()
	{
		return await periodSetFacade.GetAllPeriodSetsAsync();
	}

	protected override bool ShouldRefresh() => false;
}

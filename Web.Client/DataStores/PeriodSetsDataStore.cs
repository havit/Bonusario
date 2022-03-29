using Havit.Blazor.Components.Web.Services.DataStores;

namespace Havit.Bonusario.Web.Client.DataStores;

public class PeriodSetsDataStore : DictionaryStaticDataStore<int, PeriodSetDto>, IPeriodSetsDataStore
{
	private readonly Func<IPeriodSetFacade> PeriodSetFacade;

	public PeriodSetsDataStore(Func<IPeriodSetFacade> PeriodSetFacade)
	{
		this.PeriodSetFacade = PeriodSetFacade;
	}

	protected override Func<PeriodSetDto, int> KeySelector { get; } = (dto) => dto.Id;

	protected override async Task<IEnumerable<PeriodSetDto>> LoadDataAsync()
	{
		return await PeriodSetFacade().GetAllPeriodSetsAsync();
	}

	protected override bool ShouldRefresh() => false;
}

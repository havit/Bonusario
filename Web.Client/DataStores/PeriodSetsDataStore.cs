using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.Blazor.Components.Web.Services.DataStores;
using Havit.Bonusario.Contracts;

namespace Havit.Bonusario.Web.Client.DataStores;

public class PeriodSetsDataStore : DictionaryStaticDataStore<int, PeriodSetDto>, IPeriodSetsDataStore
{
	private readonly IPeriodSetFacade PeriodSetFacade;

	public PeriodSetsDataStore(IPeriodSetFacade PeriodSetFacade)
	{
		this.PeriodSetFacade = PeriodSetFacade;
	}

	protected override Func<PeriodSetDto, int> KeySelector { get; } = (dto) => dto.Id;

	protected override async Task<IEnumerable<PeriodSetDto>> LoadDataAsync()
	{
		return await PeriodSetFacade.GetAllPeriodSetsAsync();
	}

	protected override bool ShouldRefresh() => false;
}

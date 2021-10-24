using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.Blazor.Components.Web.Services.DataStores;
using Havit.Bonusario.Contracts;

namespace Havit.Bonusario.Web.Client.DataStores
{
	public class PeriodsDataStore : DictionaryStaticDataStore<int, PeriodDto>, IPeriodsDataStore
	{
		private readonly IPeriodFacade PeriodFacade;

		public PeriodsDataStore(IPeriodFacade PeriodFacade)
		{
			this.PeriodFacade = PeriodFacade;
		}

		protected override Func<PeriodDto, int> KeySelector { get; } = (dto) => dto.PeriodId;

		protected override async Task<IEnumerable<PeriodDto>> LoadDataAsync()
		{
			return await PeriodFacade.GetAllActivePeriodsAsync();
		}

		protected override bool ShouldRefresh() => false;
	}
}

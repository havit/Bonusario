using System.Collections.Generic;
using System.Threading.Tasks;
using Havit.Blazor.Components.Web.Services.DataStores;
using Havit.Bonusario.Contracts;

namespace Havit.Bonusario.Web.Client.DataStores;

public interface IPeriodsDataStore : IDictionaryStaticDataStore<int, PeriodDto>
{
	Task<List<PeriodDto>> GetActiveForSubmissionAsync();
	List<PeriodDto> GetActiveForSubmission();
	Task<List<PeriodDto>> GetClosedAsync();
	List<PeriodDto> GetClosed();
}

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Havit.Bonusario.Contracts;
using Havit.ComponentModel;

namespace Havit.Bonusario.Contracts
{
	[ApiContract]
	public interface IPeriodFacade
	{
		Task<List<PeriodDto>> GetAllActivePeriodsAsync(CancellationToken cancellationToken = default);
	}
}
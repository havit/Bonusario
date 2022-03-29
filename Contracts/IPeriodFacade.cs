using Havit.ComponentModel;

namespace Havit.Bonusario.Contracts;

[ApiContract]
public interface IPeriodFacade
{
	Task<List<PeriodDto>> GetAllPeriodsAsync(CancellationToken cancellationToken = default);
}

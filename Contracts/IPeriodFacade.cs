using Havit.ComponentModel;

namespace Havit.Bonusario.Contracts;

[ApiContract]
public interface IPeriodFacade
{
	Task CreateNewPeriod(PeriodDto periodDto, CancellationToken cancellationToken = default);
	Task<List<PeriodDto>> GetAllPeriodsAsync(CancellationToken cancellationToken = default);
}

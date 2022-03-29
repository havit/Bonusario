using Havit.ComponentModel;

namespace Havit.Bonusario.Contracts;

[ApiContract]
public interface IPeriodSetFacade
{
	Task<List<PeriodSetDto>> GetAllPeriodSetsAsync(CancellationToken cancellationToken = default);
}

using Havit.ComponentModel;

namespace Havit.Bonusario.Contracts.Infrastructure;

[ApiContract]
public interface IMaintenanceFacade
{
	Task ClearCache(CancellationToken cancellationToken = default);
}

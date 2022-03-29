using System.Security.Claims;
using System.Threading.Tasks;
using System.Threading;
using Havit.Bonusario.Model;

namespace Havit.Bonusario.Facades.Infrastructure.Security.Authentication;

public interface IApplicationAuthenticationService
{
	ClaimsPrincipal GetCurrentClaimsPrincipal();
	Task<Employee> GetCurrentEmployeeAsync(CancellationToken cancellationToken = default);
}

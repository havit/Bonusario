using System.Security.Claims;
using Havit.Bonusario.Model;

namespace Havit.Bonusario.Facades.Infrastructure.Security.Authentication;

public interface IApplicationAuthenticationService
{
	ClaimsPrincipal GetCurrentClaimsPrincipal();
	Task<Employee> GetCurrentEmployeeAsync(CancellationToken cancellationToken = default);
}

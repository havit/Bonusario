using System.Diagnostics;
using System.Security.Claims;
using Havit.Bonusario.Facades.Infrastructure.Security.Authentication;
using Havit.Bonusario.Model;
using Havit.Bonusario.DataLayer.Repositories;

namespace Havit.Bonusario.Web.Server.Infrastructure.Security;

/// <summary>
/// Poskytuje uživatele z HttpContextu.
/// </summary>
public class ApplicationAuthenticationService : IApplicationAuthenticationService
{
	private readonly IHttpContextAccessor httpContextAccessor;
	private readonly IEmployeeRepository employeeRepository;

	private Employee employee;

	public ApplicationAuthenticationService(IHttpContextAccessor httpContextAccessor, IEmployeeRepository employeeRepository)
	{
		this.httpContextAccessor = httpContextAccessor;
		this.employeeRepository = employeeRepository;
	}

	public ClaimsPrincipal GetCurrentClaimsPrincipal()
	{
		return httpContextAccessor.HttpContext.User;
	}

	public async Task<Employee> GetCurrentEmployeeAsync(CancellationToken cancellationToken = default)
	{
		employee ??= await employeeRepository.GetByEmailAsync(GetCurrentUserEmail(), cancellationToken);
		return employee;
	}

	public string GetCurrentUserEmail()
	{
		var principal = GetCurrentClaimsPrincipal();
		Claim claim = principal.Claims.Single(claim => (claim.Type == ClaimTypes.Upn));
		Debug.Assert(claim.Value.Contains("@"));
		return claim.Value;
	}
}

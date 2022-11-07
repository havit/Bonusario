using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Havit.Bonusario.Services.Infrastructure;
using Havit.Extensions.DependencyInjection.Abstractions;
using Microsoft.AspNetCore.Authentication;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace Havit.Bonusario.Web.Server.Infrastructure.Security;

[Service(Profile = ServiceProfiles.WebServer)]
public class ApplicationClaimsTransformation : IClaimsTransformation
{
	public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
	{
		if (principal.HasClaim(ClaimTypes.Name, "crha@havit.cz")
			|| principal.HasClaim(ClaimTypes.Name, "haken@havit.cz")
#if DEBUG
			|| principal.HasClaim(ClaimTypes.Name, "hoblik@havit.cz")
#endif
			)
		{

			ClaimsIdentity claimsIdentity = ((ClaimsIdentity)principal.Identity).Clone();
			claimsIdentity.AddClaim(new Claim("role", "Administrator"));
			ClaimsPrincipal claimsPrincipalWithCustomClaims = new ClaimsPrincipal(claimsIdentity);
			return Task.FromResult(claimsPrincipalWithCustomClaims);
		}
		return Task.FromResult(principal);
	}
}

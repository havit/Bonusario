using System.Security.Claims;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;

namespace Havit.Bonusario.Web.Client.Infrastructure.Security;

public class CustomAccountClaimsPrincipalFactory : AccountClaimsPrincipalFactory<RemoteUserAccount>
{
	public CustomAccountClaimsPrincipalFactory(IAccessTokenProviderAccessor accessor) : base(accessor)
	{
		// NOOP
	}

	public override async ValueTask<ClaimsPrincipal> CreateUserAsync(RemoteUserAccount account, RemoteAuthenticationUserOptions options)
	{
		var user = await base.CreateUserAsync(account, options);

		if (user.Identity.IsAuthenticated)
		{
			var identity = (ClaimsIdentity)user.Identity;

			if (identity.HasClaim("preferred_username", "crha@havit.cz")
				|| identity.HasClaim("preferred_username", "haken@havit.cz")
# if DEBUG
				|| identity.HasClaim("preferred_username", "hoblik@havit.cz")
#endif
				)
			{
				identity.AddClaim(new Claim("role", "Administrator"));
			}
		}

		return user;
	}
}

using System.Net;
using Havit.Bonusario.Facades.Infrastructure.Security.Authentication;
using Havit.Bonusario.Web.Server.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.Net.Http.Headers;

namespace Havit.Bonusario.Web.Server.Infrastructure.ConfigurationExtensions;

public static class AuthConfig
{
	public static void AddCustomizedAuth(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			.AddMicrosoftIdentityWebApi(configuration.GetSection("AzureAd"));

		services.Configure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
		{
			options.TokenValidationParameters.NameClaimType = "name";
			options.TokenValidationParameters.RoleClaimType = "role";
		});

		services.AddScoped<IApplicationAuthenticationService, ApplicationAuthenticationService>();

		services.ConfigureApplicationCookie(configuration =>
		{
			// Do not redirect API calls - customization to include gRPC calls
			// https://github.com/dotnet/aspnetcore/blob/main/src/Security/Authentication/Cookies/src/CookieAuthenticationEvents.cs
			configuration.Events.OnRedirectToLogin = (context) => RedirectOrApiStatus(context, HttpStatusCode.Unauthorized);
			configuration.Events.OnRedirectToAccessDenied = (context) => RedirectOrApiStatus(context, HttpStatusCode.Forbidden);
		});

		services.AddScoped<IClaimsTransformation, ApplicationClaimsTransformation>();
		services.AddScoped<IApplicationAuthenticationService, ApplicationAuthenticationService>();
	}

	private static Task RedirectOrApiStatus(RedirectContext<CookieAuthenticationOptions> context, HttpStatusCode apiStatus)
	{
		if (IsApiRequest(context.Request))
		{
			context.Response.Headers[HeaderNames.Location] = context.RedirectUri;
			context.Response.StatusCode = (int)apiStatus;
		}
		else
		{
			context.Response.Redirect(context.RedirectUri);
		}
		return Task.CompletedTask;
	}

	private static bool IsApiRequest(HttpRequest request)
	{
		return
			request.Headers[HeaderNames.ContentType].ToString().StartsWith("application/grpc", StringComparison.Ordinal) // gRPC-Web
			|| String.Equals(request.Query[HeaderNames.XRequestedWith], "XMLHttpRequest", StringComparison.Ordinal) // AJAX
			|| String.Equals(request.Headers[HeaderNames.XRequestedWith], "XMLHttpRequest", StringComparison.Ordinal); // AJAX
	}
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BlazorApplicationInsights;
using FluentValidation;
using Havit.Blazor.Components.Web;
using Havit.Blazor.Components.Web.Bootstrap;
using Havit.Blazor.Grpc.Client;
using Havit.Blazor.Grpc.Client.ServerExceptions;
using Havit.Blazor.Grpc.Client.WebAssembly;
using Havit.Bonusario.Contracts;
using Havit.Bonusario.Contracts.System;
using Havit.Bonusario.Web.Client.DataStores;
using Havit.Bonusario.Web.Client.Infrastructure.Grpc;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Havit.Bonusario.Web.Client
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebAssemblyHostBuilder.CreateDefault(args);

			AddLoggingAndApplicationInsights(builder);

			builder.RootComponents.Add<App>("app");

			builder.Services.AddHttpClient("Havit.Bonusario.Web.Server", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
				.AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();
			builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
				.CreateClient("Havit.Bonusario.Web.Server"));
			AddAuth(builder);

			builder.Services.AddValidatorsFromAssemblyContaining<Dto<object>>();

			builder.Services.AddHxServices();
			builder.Services.AddHxMessenger();
			builder.Services.AddHxMessageBoxHost();
			Havit.Bonusario.Web.Client.Resources.ResourcesServiceCollectionInstaller.AddGeneratedResourceWrappers(builder.Services);
			Havit.Bonusario.Resources.ResourcesServiceCollectionInstaller.AddGeneratedResourceWrappers(builder.Services);
			SetHxComponents();

			AddGrpcClient(builder);

			builder.Services.AddSingleton<IEmployeesDataStore, EmployeesDataStore>();
			builder.Services.AddSingleton<IPeriodsDataStore, PeriodsDataStore>();

			WebAssemblyHost webAssemblyHost = builder.Build();

			SetLanguage(webAssemblyHost);

			await webAssemblyHost.RunAsync();
		}

		private static void AddAuth(WebAssemblyHostBuilder builder)
		{
			builder.Services.AddMsalAuthentication(options =>
			{
				builder.Configuration.Bind("AzureAd", options.ProviderOptions);
				options.ProviderOptions.LoginMode = "redirect";
			});

			//builder.Services.AddScoped(typeof(AccountClaimsPrincipalFactory<RemoteUserAccount>), typeof(CustomAccountClaimsPrincipalFactory));
			builder.Services.AddApiAuthorization();
		}

		private static void SetHxComponents()
		{
			// HxProgressIndicator.DefaultDelay = 0;
		}

		private static void AddGrpcClient(WebAssemblyHostBuilder builder)
		{
			builder.Services.AddTransient<IOperationFailedExceptionGrpcClientListener, HxMessengerOperationFailedExceptionGrpcClientListener>();
			builder.Services.AddTransient<AuthorizationGrpcClientInterceptor>();
			builder.Services.AddGrpcClientInfrastructure(assemblyToScanForDataContracts: typeof(Dto).Assembly);
			builder.Services.AddGrpcClientsByApiContractAttributes(
				typeof(IDataSeedFacade).Assembly,
				configureGrpcClientWithAuthorization: grpcClient =>
				{
					grpcClient.AddHttpMessageHandler(provider =>
					{
						var navigationManager = provider.GetRequiredService<NavigationManager>();
						var backendUrl = navigationManager.BaseUri;

						return provider.GetRequiredService<AuthorizationMessageHandler>()
							.ConfigureHandler(authorizedUrls: new[] { backendUrl }); // TODO? as neede: , scopes: new[] { "havit-Bonusario-api" });
					})
					.AddInterceptor<AuthorizationGrpcClientInterceptor>();
				});
		}

		private static void SetLanguage(WebAssemblyHost webAssemblyHost)
		{
			var cultureInfo = new CultureInfo("cs-CZ");
			CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
			CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
		}

		private static void AddLoggingAndApplicationInsights(WebAssemblyHostBuilder builder)
		{
			var instrumentationKey = builder.Configuration.GetValue<string>("ApplicationInsights:InstrumentationKey");

			builder.Services.AddBlazorApplicationInsights(async applicationInsights =>
			{
				await applicationInsights.SetInstrumentationKey(instrumentationKey);
				await applicationInsights.LoadAppInsights();

				var telemetryItem = new TelemetryItem()
				{
					Tags = new Dictionary<string, object>()
					{
						{ "ai.cloud.role", "Web.Client" },
						// { "ai.cloud.roleInstance", "..." },
					}
				};

				await applicationInsights.AddTelemetryInitializer(telemetryItem);
			}, addILoggerProvider: true);

			builder.Logging.AddFilter<ApplicationInsightsLoggerProvider>(level => (level == LogLevel.Error) || (level == LogLevel.Critical));

#if DEBUG
			builder.Logging.SetMinimumLevel(LogLevel.Debug);
#endif
		}
	}
}

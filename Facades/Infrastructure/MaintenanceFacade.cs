using Havit.Extensions.DependencyInjection.Abstractions;
using Havit.Services.Caching;
using Microsoft.AspNetCore.Authorization;
using Havit.Bonusario.Contracts.Infrastructure;

namespace Havit.Bonusario.Facades.Infrastructure.System;

[Service]
[Authorize] // TODO Auth
public class MaintenanceFacade : IMaintenanceFacade
{
	private readonly ICacheService cacheService;

	public MaintenanceFacade(ICacheService cacheService)
	{
		this.cacheService = cacheService;
	}

	public Task ClearCache(CancellationToken cancellationToken = default)
	{
		cacheService.Clear();

		return Task.CompletedTask;
	}
}

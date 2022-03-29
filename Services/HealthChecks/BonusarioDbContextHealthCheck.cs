using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Havit.Data.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Havit.Bonusario.Services.HealthChecks;

/// <summary>
/// Kontroluje dostupnost databáze.
/// De facto jen pro inpiraci pro další health checks, jinak je kontrola dostupnosti databáze pomocí DbContextu vestavěna - viz
/// metoda AddDbContextCheck(nuget balíček Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore).
/// </summary>
public class BonusarioDbContextHealthCheck : BaseHealthCheck
{
	private readonly IDbContext dbContext;

	public BonusarioDbContextHealthCheck(IDbContext dbContext)
	{
		this.dbContext = dbContext;
	}

	protected async override Task<HealthCheckResult> CheckHealthAsync(CancellationToken cancellationToken)
	{
		return await dbContext.Database.CanConnectAsync(cancellationToken)
			? HealthCheckResult.Healthy()
			: HealthCheckResult.Unhealthy();
	}
}

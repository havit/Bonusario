using Havit.Data.EntityFrameworkCore;
using Havit.Data.Patterns.DataSeeds;
using Havit.Bonusario.DataLayer.Seeds.Core;
using Microsoft.EntityFrameworkCore;

namespace Havit.Bonusario.Web.Server.Tools;

public static class DatabaseMigration
{
	public static void UpgradeDatabaseSchemaAndData(this IApplicationBuilder app)
	{
		using (IServiceScope serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
		{
			var context = serviceScope.ServiceProvider.GetService<IDbContext>();
			context.Database.Migrate();

			var dataSeedRunner = serviceScope.ServiceProvider.GetService<IDataSeedRunner>();
			dataSeedRunner.SeedData<CoreProfile>();
		}
	}
}

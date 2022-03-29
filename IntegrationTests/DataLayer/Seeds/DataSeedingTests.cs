using System.Linq;
using Havit.Data.Patterns.DataSeeds;
using Havit.Bonusario.DataLayer.Seeds.Core;
using Havit.Bonusario.Model.Common;
using Havit.Data.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.DependencyInjection;
using Havit.Services.Caching;
using Havit.Bonusario.TestHelpers;

namespace Havit.Bonusario.IntegrationTests.DataLayer.Seeds;

[TestClass]
public class DataSeedingTests : IntegrationTestBase
{
	protected override bool UseLocalDb => true;
	protected override bool DeleteDbData => true; // default, but to be sure :D

	//[TestMethod]
	public void DataSeeds_CoreProfile()
	{
		// arrange
		var seedRunner = ServiceProvider.GetRequiredService<IDataSeedRunner>();

		// act
		seedRunner.SeedData<CoreProfile>();

		// assert
		// no exception
	}
}

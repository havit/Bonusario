using Havit.Bonusario.DataLayer.Seeds.CompanyWide;
using Havit.Data.Patterns.DataSeeds.Profiles;

namespace Havit.Bonusario.DataLayer.Seeds.Demo;

public class DemoProfile : DataSeedProfile
{
	public override IEnumerable<Type> GetPrerequisiteProfiles()
	{
		yield return typeof(CompanyWideDataSeedProfile);
	}
}

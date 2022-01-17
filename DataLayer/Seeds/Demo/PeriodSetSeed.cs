using Havit.Bonusario.Model;
using Havit.Data.Patterns.DataSeeds;
using Havit.Services.TimeServices;

namespace Havit.Bonusario.DataLayer.Seeds.Demo;

public class PeriodSetSeed : DataSeed<DemoProfile>
{
	private readonly ITimeService timeService;

	public PeriodSetSeed(ITimeService timeService)
	{
		this.timeService = timeService;
	}

	public override void SeedData()
	{
		var data = new PeriodSet[]
			{
				new PeriodSet()
				{
					Name = "Období 1",
					Budget = 100_000,
					Created = timeService.GetCurrentTime(),
				}
			};

		Seed(For(data).PairBy(ps => ps.Name).WithoutUpdate());
	}
}

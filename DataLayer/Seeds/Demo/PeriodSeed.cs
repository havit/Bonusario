using Havit.Bonusario.Model;
using Havit.Data.Patterns.DataSeeds;
using Havit.Services.TimeServices;

namespace Havit.Bonusario.DataLayer.Seeds.Demo;

public class PeriodSeed : DataSeed<DemoProfile>
{
	private readonly ITimeService timeService;

	public PeriodSeed(ITimeService timeService)
	{
		this.timeService = timeService;
	}

	public override void SeedData()
	{
		var date = timeService.GetCurrentDate().AddMonths(-2);

		var periods = new List<Period>();

		// 2x předchozí + aktuální + budoucí
		for (int i = 0; i < 4; i++)
		{
			periods.Add(new Period()
			{
				Name = date.Month + "/" + date.Year,
				StartDate = new DateTime(date.Year, date.Month, 1),
				EndDate = new DateTime(date.Year, date.Month, Math.Max(date.Day, 11)).AddMonths(1).AddDays(-1), // do 10. následujícícho měsíce, nebo dříve (DEMO)
				PeriodSetId = 1,
				Created = timeService.GetCurrentTime(),
			});
			date = date.AddMonths(1);
		};

		Seed(For(periods.ToArray()).PairBy(p => p.Name));
	}

	public override IEnumerable<Type> GetPrerequisiteDataSeeds()
	{
		yield return typeof(PeriodSetSeed);
	}
}

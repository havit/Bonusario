using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.Bonusario.DataLayer.Seeds.Core;
using Havit.Bonusario.Model;
using Havit.Data.Patterns.DataSeeds;
using Havit.Services.TimeServices;

namespace Havit.Bonusario.DataLayer.Seeds.Core
{
	public class PeriodSeed : DataSeed<CoreProfile>
	{
		private readonly ITimeService timeService;

		public PeriodSeed(ITimeService timeService)
		{
			this.timeService = timeService;
		}

		public override void SeedData()
		{
			var date = timeService.GetCurrentDate();

			var periods = new List<Period>();

			for (int i = 0; i < 1; i++)
			{
				periods.Add(new Period()
				{
					Name = date.Month + "/" + date.Year,
					StartDate = new DateTime(date.Year, date.Month, 1),
					EndDate = new DateTime(date.Year, date.Month, 1).AddMonths(1).AddDays(-1),
					Created = timeService.GetCurrentTime(),
				});
				date = date.AddMonths(1);
			};

			Seed(For(periods.ToArray()).PairBy(p => p.Name));
		}
	}
}

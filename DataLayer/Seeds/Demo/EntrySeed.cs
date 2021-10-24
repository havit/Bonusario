using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.Bonusario.DataLayer.Repositories;
using Havit.Bonusario.Model;
using Havit.Data.Patterns.DataSeeds;
using Havit.Services.TimeServices;

namespace Havit.Bonusario.DataLayer.Seeds.Demo
{
	public class EntrySeed : DataSeed<DemoProfile>
	{
		private readonly ITimeService timeService;
		private readonly IEmployeeRepository employeeRepository;
		private readonly IPeriodRepository periodRepository;

		public EntrySeed(ITimeService timeService, IEmployeeRepository employeeRepository, IPeriodRepository periodRepository)
		{
			this.timeService = timeService;
			this.employeeRepository = employeeRepository;
			this.periodRepository = periodRepository;
		}
		public override void SeedData()
		{
			var now = timeService.GetCurrentTime();
			var employees = employeeRepository.GetAll();
			var periods = periodRepository.GetAll();
			var currentPeriod = periods.Single(p => p.Name == now.Month + "/" + now.Year);

			var entries = new[]
			{
				new Entry()
				{
					CreatedById = employees.Single(e => e.Email == "haken@havit.cz").Id,
					RecipientId = employees.Single(e => e.Email == "crha@havit.cz").Id,
					PeriodId = currentPeriod.Id,
					Text = "Díky za pomoc s Havit.Blazor, bez tebe bychom to dohromady nedali.",
					Value = 13,
					Tags =
					{
						new EntryTag() { Tag = "team" },
						new EntryTag() { Tag = "passion" },
						new EntryTag() { Tag = "knowledge" }
					},
					Created = now
				},
				new Entry()
				{
					CreatedById = employees.Single(e => e.Email == "haken@havit.cz").Id,
					RecipientId = employees.Single(e => e.Email == "hruby@havit.cz").Id,
					PeriodId = currentPeriod.Id,
					Value = 15,
					Created = now
				},
				new Entry()
				{
					CreatedById = employees.Single(e => e.Email == "haken@havit.cz").Id,
					RecipientId = employees.Single(e => e.Email == "kanda@havit.cz").Id,
					PeriodId = currentPeriod.Id,
					Value = 15,
					Created = now
				}
			};

			Seed(For(entries)
				.PairBy(e => e.CreatedById, e => e.RecipientId, e => e.PeriodId, e => e.Value, e => e.Text)
				.AfterSave(e => e.SeedEntity.Tags.ForEach(t => t.EntryId = e.PersistedEntity.Id))
				.AndForAll(e => e.Tags, configuration =>
					{
						configuration.PairBy(t => t.EntryId, t => t.Tag);
					}));
		}

		public override IEnumerable<Type> GetPrerequisiteDataSeeds()
		{
			yield return typeof(PeriodSeed);
		}
	}
}

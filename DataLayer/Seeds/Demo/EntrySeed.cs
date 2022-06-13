using Havit.Bonusario.DataLayer.Repositories;
using Havit.Bonusario.Model;
using Havit.Bonusario.Primitives;
using Havit.Data.Patterns.DataSeeds;
using Havit.Services.TimeServices;

namespace Havit.Bonusario.DataLayer.Seeds.Demo;

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
		var employees = employeeRepository.GetAll();
		var periods = periodRepository.GetAll();

		var now = timeService.GetCurrentTime();
		var previousDate = now.AddMonths(-1);
		var previousPreviousDate = now.AddMonths(-2);
		var previousPeriod = periods.Single(p => p.Name == previousDate.Month + "/" + previousDate.Year);
		var previousPreviousPeriod = periods.Single(p => p.Name == previousPreviousDate.Month + "/" + previousDate.Year);
		var currentPeriod = periods.Single(p => p.Name == now.Month + "/" + now.Year);

		var entries = new[]
		{
			new Entry()
			{
				CreatedById = employees.Single(e => e.Email == "haken@havit.cz").Id,
				RecipientId = employees.Single(e => e.Email == "crha@havit.cz").Id,
				PeriodId = previousPreviousPeriod.Id,
				Text = "Pokusníci",
				Value = 3,
				Tags =
				{
					new EntryTag() { Tag = "team" },
					new EntryTag() { Tag = "znalosti" }
				},
				Created = now.AddMonths(-2),
				Submitted = now.AddMonths(-2),
				AuthorIdentityVisibility = AuthorIdentityVisibility.Public
			},
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
					new EntryTag() { Tag = "vášeň" },
					new EntryTag() { Tag = "znalosti" }
				},
				Created = now,
				AuthorIdentityVisibility = AuthorIdentityVisibility.VisibleOnlyForRecipient
			},
			new Entry()
			{
				CreatedById = employees.Single(e => e.Email == "crha@havit.cz").Id,
				RecipientId = employees.Single(e => e.Email == "haken@havit.cz").Id,
				PeriodId = previousPeriod.Id,
				Text = "Lorem ipsum sut neram, dot per gaham.",
				Submitted = previousDate,
				Value = 13,
				Tags =
				{
					new EntryTag() { Tag = "team" },
					new EntryTag() { Tag = "znalosti" }
				},
				Created = now.AddMinutes(-123),
				AuthorIdentityVisibility = AuthorIdentityVisibility.Hidden
			},
			new Entry()
			{
				CreatedById = employees.Single(e => e.Email == "hruby@havit.cz").Id,
				RecipientId = employees.Single(e => e.Email == "haken@havit.cz").Id,
				PeriodId = previousPeriod.Id,
				Submitted = previousDate,
				Value = 15,
				Created = now,
				AuthorIdentityVisibility = AuthorIdentityVisibility.Hidden
			},
			new Entry()
			{
				CreatedById = employees.Single(e => e.Email == "haken@havit.cz").Id,
				RecipientId = employees.Single(e => e.Email == "hruby@havit.cz").Id,
				PeriodId = currentPeriod.Id,
				Value = 15,
				Created = now.AddMinutes(-32),
				AuthorIdentityVisibility = AuthorIdentityVisibility.Public
			},
			new Entry()
			{
				CreatedById = employees.Single(e => e.Email == "haken@havit.cz").Id,
				RecipientId = employees.Single(e => e.Email == "kanda@havit.cz").Id,
				PeriodId = previousPeriod.Id,
				Submitted = previousDate,
				Tags =
				{
					new EntryTag() { Tag = "team" },
					new EntryTag() { Tag = "znalosti" }
				},
				Value = 15,
				Created = now,
				AuthorIdentityVisibility = AuthorIdentityVisibility.VisibleOnlyForRecipient
			},
			new Entry()
			{
				CreatedById = employees.Single(e => e.Email == "haken@havit.cz").Id,
				RecipientId = employees.Single(e => e.Email == "kanda@havit.cz").Id,
				PeriodId = currentPeriod.Id,
				Value = 15,
				Created = now,
				AuthorIdentityVisibility = AuthorIdentityVisibility.Public
			},
			new Entry()
			{
				CreatedById = employees.Single(e => e.Email == "haken@havit.cz").Id,
				RecipientId = employees.Single(e => e.Email == "gregor@havit.cz").Id,
				PeriodId = currentPeriod.Id,
				Text = "Líbí se mi, že ses ujal organizace lyžarské akce.",
				Value = 3,
				Submitted = now,
				Tags =
				{
					new EntryTag() { Tag = "team" },
					new EntryTag() { Tag = "potěšení" },
				},
				Created = now,
				AuthorIdentityVisibility = AuthorIdentityVisibility.Hidden
			},
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

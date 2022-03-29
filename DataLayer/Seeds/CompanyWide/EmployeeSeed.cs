using Havit.Bonusario.Model;
using Havit.Data.Patterns.DataSeeds;
using Havit.Services.TimeServices;

namespace Havit.Bonusario.DataLayer.Seeds.CompanyWide;

public class EmployeeSeed : DataSeed<CompanyWideDataSeedProfile>
{
	private readonly ITimeService timeService;

	public EmployeeSeed(ITimeService timeService)
	{
		this.timeService = timeService;
	}

	public override void SeedData()
	{
		var employees = new Employee[]
		{
			new Employee() { Name = "Jan Babušík", Email = "babusik@havit.cz" },
			new Employee() { Name = "Dominik Crha", Email = "crha@havit.cz" },
			new Employee() { Name = "Gabriela Čílová", Email = "cilova@havit.cz" },
			//new Employee() { Name = "Jiří Činčura", Email = "cincura@havit.cz" },
			new Employee() { Name = "Renáta Drozdová", Email = "drozdova@havit.cz" },
			new Employee() { Name = "Jiří Gregor", Email = "gregor@havit.cz" },
			new Employee() { Name = "Alexandr Hájek", Email = "hajek@havit.cz" },
			new Employee() { Name = "Robert Haken", Email = "haken@havit.cz" },
			new Employee() { Name = "Zbyšek Hlinka", Email = "hlinka@havit.cz" },
			//new Employee() { Name = "Mikuláš Hoblík", Email = "hoblik@havit.cz" },
			new Employee() { Name = "Daniel Hrubý", Email = "hruby@havit.cz" },
			new Employee() { Name = "Jiří Kanda", Email = "kanda@havit.cz" },
			new Employee() { Name = "Miloslav Kašpárek", Email = "kasparek@havit.cz" },
			new Employee() { Name = "Martin Kochman", Email = "kochman@havit.cz" },
			new Employee() { Name = "Viliam Kortiš", Email = "kortis@havit.cz" },
			new Employee() { Name = "Pavel Kříž", Email = "kriz@havit.cz" },
			new Employee() { Name = "Miroslav Louma", Email = "louma@havit.cz" },
			new Employee() { Name = "Michael Melena", Email = "melena@havit.cz" },
			new Employee() { Name = "Jakub Nábělek", Email = "nabelek@havit.cz" },
			new Employee() { Name = "Pavel Růžička", Email = "ruzicka@havit.cz" },
			new Employee() { Name = "Petr Svoboda", Email = "svoboda@havit.cz" },
			new Employee() { Name = "Jiří Šáda", Email = "sada@havit.cz" },
			new Employee() { Name = "Ondřej Václavek", Email = "vaclavek@havit.cz" },
			new Employee() { Name = "Igor Vít", Email = "vit@havit.cz" },
			new Employee() { Name = "Tomáš Wagner", Email = "wagner@havit.cz" },
		};

		foreach (var employee in employees)
		{
			employee.Created = timeService.GetCurrentTime();
		}

		Seed(For(employees).PairBy(e => e.Email));
	}
}

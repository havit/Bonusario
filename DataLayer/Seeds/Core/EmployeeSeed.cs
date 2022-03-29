using Havit.Bonusario.Model;
using Havit.Data.Patterns.DataSeeds;
using Havit.Services.TimeServices;

namespace Havit.Bonusario.DataLayer.Seeds.Core;

public class EmployeeSeed : DataSeed<CoreProfile>
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
			new Employee() { Name = "Dominik Crha", Email = "crha@havit.cz" },
			new Employee() { Name = "Robert Haken", Email = "haken@havit.cz" },
			new Employee() { Name = "Daniel Hrubý", Email = "hruby@havit.cz" },
			new Employee() { Name = "Jiří Kanda", Email = "kanda@havit.cz" },
			new Employee() { Name = "Martin Kochman", Email = "kochman@havit.cz" },
			new Employee() { Name = "Igor Vít", Email = "vit@havit.cz" },
		};

		foreach (var employee in employees)
		{
			employee.Created = timeService.GetCurrentTime();
		}

		Seed(For(employees).PairBy(e => e.Email));
	}
}

namespace Havit.Bonusario.Model;

public class PeriodSet
{
	public int Id { get; set; }

	[MaxLength(50)]
	public string Name { get; set; }

	public List<Period> Periods { get; } = new List<Period>();

	public decimal Budget { get; set; }

	public DateTime Created { get; set; }
}

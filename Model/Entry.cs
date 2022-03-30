namespace Havit.Bonusario.Model;

public class Entry
{
	public int Id { get; set; }

	public Employee CreatedBy { get; set; }
	public int CreatedById { get; set; }

	public Employee Recipient { get; set; }
	public int RecipientId { get; set; }

	public Period Period { get; set; }
	public int PeriodId { get; set; }

	[MaxLength(Int32.MaxValue)]
	public string Text { get; set; }

	public int Value { get; set; }

	public List<EntryTag> Tags { get; } = new List<EntryTag>();

	public DateTime? Submitted { get; set; }

	public DateTime Created { get; set; }

	public DateTime? Deleted { get; set; }
}

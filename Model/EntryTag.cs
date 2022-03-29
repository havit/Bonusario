namespace Havit.Bonusario.Model;

public class EntryTag
{
	public int Id { get; set; }

	public Entry Entry { get; set; }
	public int EntryId { get; set; }

	[MaxLength(100)]
	public string Tag { get; set; }
}

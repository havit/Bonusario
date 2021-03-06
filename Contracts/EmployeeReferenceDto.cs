using Havit.Bonusario.Primitives;

namespace Havit.Bonusario.Contracts;

public class EmployeeReferenceDto
{
	public int EmployeeId { get; set; }
	public string Name { get; set; }
	public string Email { get; set; }
	public EntryVisibility DefaultVisibility { get; set; }
	public bool IsDeleted { get; set; }
}

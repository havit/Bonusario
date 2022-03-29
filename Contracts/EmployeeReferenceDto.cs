using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Bonusario.Contracts;

public class EmployeeReferenceDto
{
	public int EmployeeId { get; set; }
	public string Name { get; set; }
	public string Email { get; set; }
	public bool IsDeleted { get; set; }
}

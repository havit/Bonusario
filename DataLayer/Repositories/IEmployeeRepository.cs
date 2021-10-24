using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.Data.Patterns.Repositories;
using Havit.Bonusario.Model;
using System.Threading;

namespace Havit.Bonusario.DataLayer.Repositories
{
	public partial interface IEmployeeRepository
	{
		Task<Employee> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
	}
}
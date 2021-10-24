using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.Data.EntityFrameworkCore;
using Havit.Data.EntityFrameworkCore.Patterns.Repositories;
using Havit.Data.EntityFrameworkCore.Patterns.SoftDeletes;
using Havit.Data.Patterns.DataEntries;
using Havit.Data.Patterns.DataLoaders;
using Havit.Bonusario.Model;
using System.Threading;
using Microsoft.EntityFrameworkCore;

namespace Havit.Bonusario.DataLayer.Repositories
{
	public partial class EmployeeDbRepository : IEmployeeRepository
	{
		public Task<Employee> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
		{
			return Data
				.Include(GetLoadReferences)
				.FirstAsync(e => e.Email == email, cancellationToken);
		}

		public Task<List<Employee>> GetAllIncludingDeletedAsync(CancellationToken cancellationToken = default)
		{
			return DataIncludingDeleted
				.Include(GetLoadReferences)
				.ToListAsync(cancellationToken);
		}
	}
}

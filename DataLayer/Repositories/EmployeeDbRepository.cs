using Havit.Data.EntityFrameworkCore.Patterns.Repositories;
using Havit.Bonusario.Model;
using Microsoft.EntityFrameworkCore;

namespace Havit.Bonusario.DataLayer.Repositories;

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

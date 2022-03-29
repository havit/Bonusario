using Havit.Bonusario.Model;

namespace Havit.Bonusario.DataLayer.Repositories;

public partial interface IEmployeeRepository
{
	Task<Employee> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
	Task<List<Employee>> GetAllIncludingDeletedAsync(CancellationToken cancellationToken = default);
}

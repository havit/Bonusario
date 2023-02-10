using Havit.ComponentModel;

namespace Havit.Bonusario.Contracts;

[ApiContract]
public interface IEmployeeFacade
{
	Task<Dto<int>> GetCurrentEmployeeId(CancellationToken cancellationToken = default);
	Task<List<EmployeeReferenceDto>> GetAllEmployeeReferencesAsync(CancellationToken cancellationToken = default);
}

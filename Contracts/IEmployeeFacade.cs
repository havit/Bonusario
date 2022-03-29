using Havit.ComponentModel;

namespace Havit.Bonusario.Contracts;

[ApiContract]
public interface IEmployeeFacade
{
	Task<List<EmployeeReferenceDto>> GetAllEmployeeReferencesAsync(CancellationToken cancellationToken = default);
}

using Havit.ComponentModel;

namespace Havit.Bonusario.Contracts;

[ApiContract]
public interface IEmployeeFacade
{
	Task<List<EmployeeReferenceDto>> GetAllEmployeeReferencesAsync(CancellationToken cancellationToken = default);
	Task UpdateCurrentEmployeeDefaultIdentityVisibility(Dto<int> defaultVisibility, CancellationToken cancellationToken = default);
	Task<Dto<int>> GetCurrentEmployeeDefaultIdentityVisibility(CancellationToken cancellationToken = default);
}

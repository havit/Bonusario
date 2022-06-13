using Havit.Bonusario.Primitives;
using Havit.ComponentModel;

namespace Havit.Bonusario.Contracts;

[ApiContract]
public interface IEmployeeFacade
{
	Task<List<EmployeeReferenceDto>> GetAllEmployeeReferencesAsync(CancellationToken cancellationToken = default);
	Task UpdateCurrentEmployeeDefaultIdentityVisibility(Dto<AuthorIdentityVisibility> defaultVisibility, CancellationToken cancellationToken = default);
	Task<Dto<AuthorIdentityVisibility>> GetCurrentEmployeeDefaultIdentityVisibility(CancellationToken cancellationToken = default);
}

using Havit.Bonusario.Primitives;
using Havit.ComponentModel;

namespace Havit.Bonusario.Contracts;

[ApiContract]
public interface IEmployeeFacade
{
	Task<List<EmployeeReferenceDto>> GetAllEmployeeReferencesAsync(CancellationToken cancellationToken = default);
	Task UpdateCurrentEmployeeDefaultEntryVisibility(Dto<EntryVisibility> defaultVisibility, CancellationToken cancellationToken = default);
	Task<Dto<EntryVisibility>> GetCurrentEmployeeDefaultEntryVisibility(CancellationToken cancellationToken = default);
}

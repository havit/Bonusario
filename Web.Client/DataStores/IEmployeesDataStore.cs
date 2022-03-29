using Havit.Blazor.Components.Web.Services.DataStores;

namespace Havit.Bonusario.Web.Client.DataStores;

public interface IEmployeesDataStore : IDictionaryStaticDataStore<int, EmployeeReferenceDto>
{
}

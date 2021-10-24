using Havit.Blazor.Components.Web.Services.DataStores;
using Havit.Bonusario.Contracts;

namespace Havit.Bonusario.Web.Client.DataStores
{
	public interface IEmployeesDataStore : IDictionaryStaticDataStore<int, EmployeeReferenceDto>
	{
	}
}
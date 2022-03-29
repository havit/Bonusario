using Havit.Blazor.Components.Web.Services.DataStores;

namespace Havit.Bonusario.Web.Client.DataStores;

public class EmployeesDataStore : DictionaryStaticDataStore<int, EmployeeReferenceDto>, IEmployeesDataStore
{
	private readonly IEmployeeFacade employeeFacade;

	public EmployeesDataStore(IEmployeeFacade employeeFacade)
	{
		this.employeeFacade = employeeFacade;
	}

	protected override Func<EmployeeReferenceDto, int> KeySelector { get; } = (dto) => dto.EmployeeId;

	protected override async Task<IEnumerable<EmployeeReferenceDto>> LoadDataAsync()
	{
		return await employeeFacade.GetAllEmployeeReferencesAsync();
	}

	protected override bool ShouldRefresh() => false;
}

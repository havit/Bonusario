using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.Blazor.Components.Web.Services.DataStores;
using Havit.Bonusario.Contracts;

namespace Havit.Bonusario.Web.Client.DataStores
{
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
}

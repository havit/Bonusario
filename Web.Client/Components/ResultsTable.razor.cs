using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.Blazor.Components.Web.Bootstrap;
using Havit.Bonusario.Contracts;
using Havit.Bonusario.Web.Client.DataStores;
using Microsoft.AspNetCore.Components;

namespace Havit.Bonusario.Web.Client.Components
{
	public partial class ResultsTable
	{
		[Parameter] public int? PeriodId { get; set; }

		[Inject] protected IEntryFacade EntryFacade { get; set; }
		[Inject] protected IEmployeesDataStore EmployeesDataStore { get; set; }

		private List<ResultItemDto> data;
		private HxGrid<ResultItemDto> gridComponent;

		protected override async Task OnParametersSetAsync()
		{
			await EmployeesDataStore.EnsureDataAsync();
			//await gridComponent?.RefreshDataAsync();
		}

		private async Task<GridDataProviderResult<ResultItemDto>> GetDataAsync(GridDataProviderRequest<ResultItemDto> request)
		{
			try
			{
				data = await EntryFacade.GetResultsAsync(Dto.FromValue(PeriodId.Value));
				return request.ApplyTo(data);
			}
			catch (OperationFailedException)
			{
				return new() { Data = null, TotalCount = 0 };
			}
		}
	}
}

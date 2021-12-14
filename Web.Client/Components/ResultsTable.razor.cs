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
		[Parameter] public bool Aggregate { get; set; }

		[Inject] protected IEntryFacade EntryFacade { get; set; }
		[Inject] protected IEmployeesDataStore EmployeesDataStore { get; set; }

		private List<ResultItemDto> data;
		private HxGrid<ResultItemDto> gridComponent;
		private int? loadedPeriodId;
		private decimal grandTotal;

		protected override async Task OnParametersSetAsync()
		{
			await EmployeesDataStore.EnsureDataAsync();

			if ((loadedPeriodId != PeriodId) && (gridComponent != null))
			{
				await gridComponent.RefreshDataAsync();
			}
		}

		private async Task<GridDataProviderResult<ResultItemDto>> GetDataAsync(GridDataProviderRequest<ResultItemDto> request)
		{
			try
			{
				if (!Aggregate)
				{
					data = await EntryFacade.GetResultsAsync(Dto.FromValue(PeriodId.Value));
					loadedPeriodId = PeriodId;
					grandTotal = data.Sum(i => i.ValueSum);
					return request.ApplyTo(data);
				}
				else
				{
					data = await EntryFacade.GetAggregateResultsAsync();
					grandTotal = data.Sum(i => i.ValueSum);
					return request.ApplyTo(data);
				}
			}
			catch (OperationFailedException)
			{
				return new() { Data = null, TotalCount = 0 };
			}
		}
	}
}

using Havit.Bonusario.Web.Client.DataStores;
using Microsoft.AspNetCore.Components.Authorization;

namespace Havit.Bonusario.Web.Client.Components;

public partial class GivenPointsSummary
{
	[Parameter] public int? PeriodId { get; set; }

	[Inject] protected IEmployeesDataStore EmployeesDataStore { get; set; }
	[Inject] protected Func<IEntryFacade> EntryFacade { get; set; }
	[Inject] protected IHxMessageBoxService MessageBox { get; set; }
	[Inject] protected AuthenticationStateProvider AuthenticationStateProvider { get; set; }

	private IEnumerable<EmployeeReferenceDto> employees;
	private List<EntryDto> entries;

	protected override async Task OnInitializedAsync()
	{
		employees ??= await EmployeesDataStore.GetAllAsync();
	}

	protected override async Task OnParametersSetAsync()
	{
		await LoadData();
	}

	private async Task LoadData()
	{
		if (PeriodId != null)
		{
			var e = await EntryFacade().GetMyGivenEntriesAsync(Dto.FromValue(PeriodId.Value));
			entries = e.OrderByDescending(e => e.Created).ToList();
			employees = employees.OrderByDescending(e => GetPointsGivenToEmployee(e)).ThenBy(e => e.Name);
		}
	}

	private int GetPointsGivenToEmployee(EmployeeReferenceDto employee)
	{
		return entries?.Where(e => e.RecipientId == employee.EmployeeId).Sum(e => e.Value) ?? 0;
	}
}

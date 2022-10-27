﻿using System.Linq;
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

	private List<EmployeeInformation> employeeData;
	private IEnumerable<EmployeeReferenceDto> employees;

	protected override async Task OnInitializedAsync()
	{
		employees ??= await EmployeesDataStore.GetAllAsync();
	}

	public async Task ReloadData()
	{
		await LoadData();
		StateHasChanged();
	}

	protected override async Task OnParametersSetAsync()
	{
		await LoadData();
	}

	private async Task LoadData()
	{
		if (PeriodId != null)
		{
			var entries = await EntryFacade().GetMyGivenEntriesAsync(Dto.FromValue(PeriodId.Value));

			employeeData = new();

			// Calculate points distributed to each employee.
			foreach (var employee in employees)
			{
				EmployeeInformation employeeInformation = new()
				{
					EmployeeDto = employee,
					Points = entries?.Where(e => e.RecipientId == employee.EmployeeId).Sum(e => e.Value) ?? 0
				};

				employeeData.Add(employeeInformation);
			}

			employeeData = employeeData.OrderByDescending(e => e.Points).ThenBy(e => e.EmployeeDto.Name).ToList();
		}
	}

	/// <summary>
	/// Stores information about an employee including received points from the currently signed-in employee.
	/// </summary>
	private class EmployeeInformation
	{
		public EmployeeReferenceDto EmployeeDto { get; set; }
		public int Points { get; set; }
	}
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.Bonusario.Contracts;
using Havit.Bonusario.Web.Client.DataStores;
using Microsoft.AspNetCore.Components;

namespace Havit.Bonusario.Web.Client.Components
{
	public partial class EntriesBoard
	{
		[Parameter] public int PeriodId { get; set; }

		[Inject] protected IEmployeesDataStore EmployeesDataStore { get; set; }
		[Inject] protected IEntryFacade EntryFacade { get; set; }

		private IEnumerable<EmployeeReferenceDto> employees;
		private List<EntryDto> entries;
		private int? remainingPoints;

		protected override async Task OnInitializedAsync()
		{
			employees ??= await EmployeesDataStore.GetAllAsync();
			await LoadData();
		}

		protected override async Task OnParametersSetAsync()
		{
			await LoadData();
		}

		private async Task LoadData()
		{
			var e = await EntryFacade.GetMyEntriesAsync(Dto.FromValue(PeriodId));
			entries = e.OrderByDescending(e => e.Created).ToList();

			remainingPoints = (await EntryFacade.GetMyRemainingPoints(Dto.FromValue(PeriodId))).Value;
		}

		private async Task HandleEntryDeleted()
		{
			await LoadData();
		}

		private async Task HandleEntryUpdated()
		{
			await LoadData();
		}

		private async Task HandleEntryCreated()
		{
			await LoadData();
		}
	}
}
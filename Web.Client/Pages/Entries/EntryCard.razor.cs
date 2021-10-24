﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.Bonusario.Contracts;
using Havit.Bonusario.Web.Client.DataStores;
using Havit.Diagnostics.Contracts;
using Microsoft.AspNetCore.Components;

namespace Havit.Bonusario.Web.Client.Pages.Entries
{
	public partial class EntryCard
	{
		[Parameter] public EntryDto Entry { get; set; }
		[Parameter] public EventCallback OnEntryDeleted { get; set; }
		[Parameter] public EventCallback<EntryDto> OnEntryCreated { get; set; }

		[Inject] protected IEmployeesDataStore EmployeesDataStore { get; set; }
		[Inject] protected IEntryFacade EntryFacade { get; set; }


		protected override async Task OnInitializedAsync()
		{
			await EmployeesDataStore.EnsureDataAsync();
		}

		private async Task HandleDeleteClick()
		{
			Contract.Assert(Entry.Submitted is null, "Nelze smazat odeslaný záznam.");
			await EntryFacade.DeleteEntryAsync(Dto.FromValue(Entry.Id));
			await OnEntryDeleted.InvokeAsync();
		}

		private async Task HandleNewClick()
		{
			Contract.Assert(Entry.Id == default, "Záznam již není nový.");
			Contract.Assert(Entry.PeriodId != default, "PeriodId musí být nastaven.");

			try
			{
				this.Entry.Id = (await EntryFacade.CreateEntryAsync(this.Entry)).Value;
				await OnEntryCreated.InvokeAsync(this.Entry);
			}
			catch (OperationFailedException)
			{
				// NOOP
			}
		}
	}
}
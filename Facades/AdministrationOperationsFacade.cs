﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.Bonusario.Contracts;
using Havit.Extensions.DependencyInjection.Abstractions;
using Microsoft.AspNetCore.Authorization;

namespace Havit.Bonusario.Facades;

[Service]
[Authorize]
public class AdministrationOperationsFacade : IAdministrationOperationsFacade
{
	private readonly IEntryFacade entryFacade;
	private readonly IPeriodFacade periodFacade;

	public AdministrationOperationsFacade(IEntryFacade entryFacade, IPeriodFacade periodFacade)
	{
		this.entryFacade = entryFacade;
		this.periodFacade = periodFacade;
	}

	[Authorize(Roles = "Administrator")]
	public async Task SubmitEntriesAndOpenNewPeriod(CancellationToken cancellationToken = default)
	{
		PeriodDto newestPeriod = (await periodFacade.GetAllPeriodsAsync(cancellationToken)).MaxBy(p => p.EndDate);

		// Submit all entries in the last period.

		var entries = await entryFacade.GetEntriesOfPeriod(Dto.FromValue(newestPeriod.PeriodId), cancellationToken);
		var entryIds = entries.Where(e => e.Submitted is null).Select(e => e.Id).ToList();

		if (entryIds is not null && entryIds.Any())
		{
			await entryFacade.SubmitEntriesAsync(entryIds, cancellationToken);
		}

		// Create new period following the last period.

		PeriodDto newPeriod = new()
		{
			StartDate = newestPeriod.StartDate.AddMonths(1)
		};
		newPeriod.EndDate = new DateTime(
			newPeriod.StartDate.Year,
			newPeriod.StartDate.Month,
			DateTime.DaysInMonth(newPeriod.StartDate.Year, newPeriod.StartDate.Month));

		newPeriod.Name = $"{newPeriod.StartDate.Month}/{newPeriod.StartDate.Year}";

		await periodFacade.CreateNewPeriod(newPeriod, cancellationToken);
	}
}

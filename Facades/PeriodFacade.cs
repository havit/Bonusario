using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Havit.Bonusario.Contracts;
using Havit.Bonusario.DataLayer.Repositories;
using Havit.Extensions.DependencyInjection.Abstractions;
using Havit.Services.TimeServices;
using Microsoft.AspNetCore.Authorization;

namespace Havit.Bonusario.Facades
{
	[Service]
	[Authorize]
	public class PeriodFacade : IPeriodFacade
	{
		private readonly IPeriodRepository periodRepository;
		private readonly ITimeService timeService;

		public PeriodFacade(IPeriodRepository periodRepository, ITimeService timeService)
		{
			this.periodRepository = periodRepository;
			this.timeService = timeService;
		}

		public async Task<List<PeriodDto>> GetAllActivePeriodsAsync(CancellationToken cancellationToken = default)
		{
			var today = timeService.GetCurrentDate();

			var data = await periodRepository.GetAllAsync(cancellationToken);
			return data
				.Where(p => p.StartDate <= today)
				.Select(period => new PeriodDto()
				{
					PeriodId = period.Id,
					Name = period.Name,
					StartDate = period.StartDate,
					EndDate = period.EndDate
				})
				.ToList();
		}
	}
}

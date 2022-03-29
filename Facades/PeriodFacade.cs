using Havit.Bonusario.Contracts;
using Havit.Bonusario.DataLayer.Repositories;
using Havit.Extensions.DependencyInjection.Abstractions;
using Microsoft.AspNetCore.Authorization;

namespace Havit.Bonusario.Facades;

[Service]
[Authorize]
public class PeriodFacade : IPeriodFacade
{
	private readonly IPeriodRepository periodRepository;

	public PeriodFacade(IPeriodRepository periodRepository)
	{
		this.periodRepository = periodRepository;
	}

	public async Task<List<PeriodDto>> GetAllPeriodsAsync(CancellationToken cancellationToken = default)
	{
		var data = await periodRepository.GetAllAsync(cancellationToken);
		return data
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

using Havit.Bonusario.Contracts;
using Havit.Bonusario.DataLayer.Repositories;
using Havit.Extensions.DependencyInjection.Abstractions;
using Microsoft.AspNetCore.Authorization;

namespace Havit.Bonusario.Facades;

[Service]
[Authorize]
public class PeriodSetFacade : IPeriodSetFacade
{
	private readonly IPeriodSetRepository PeriodSetRepository;

	public PeriodSetFacade(IPeriodSetRepository PeriodSetRepository)
	{
		this.PeriodSetRepository = PeriodSetRepository;
	}

	public async Task<List<PeriodSetDto>> GetAllPeriodSetsAsync(CancellationToken cancellationToken = default)
	{
		var data = await PeriodSetRepository.GetAllAsync(cancellationToken);
		return data
			.Select(ps => new PeriodSetDto()
			{
				Id = ps.Id,
				Name = ps.Name,
				Budget = ps.Budget,
			})
			.ToList();
	}
}

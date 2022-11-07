using Havit.Bonusario.Contracts;
using Havit.Bonusario.DataLayer.Repositories;
using Havit.Bonusario.Model;
using Havit.Data.Patterns.UnitOfWorks;
using Havit.Extensions.DependencyInjection.Abstractions;
using Microsoft.AspNetCore.Authorization;

namespace Havit.Bonusario.Facades;

[Service]
[Authorize]
public class PeriodFacade : IPeriodFacade
{
	private readonly IPeriodRepository periodRepository;
	private readonly IUnitOfWork unitOfWork;

	public PeriodFacade(IPeriodRepository periodRepository, IUnitOfWork unitOfWork)
	{
		this.periodRepository = periodRepository;
		this.unitOfWork = unitOfWork;
	}

	[Authorize(Roles = "Administrator")]
	public async Task CreateNewPeriod(PeriodDto periodDto, CancellationToken cancellationToken = default)
	{
		Period period = new()
		{
			Name = periodDto.Name,
			StartDate = periodDto.StartDate,
			EndDate = periodDto.EndDate,
			Created = DateTime.Now
		};

		unitOfWork.AddForInsert(period);

		await unitOfWork.CommitAsync(cancellationToken);
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

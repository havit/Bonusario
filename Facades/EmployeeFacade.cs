using Havit.Bonusario.Contracts;
using Havit.Bonusario.DataLayer.Repositories;
using Havit.Bonusario.Facades.Infrastructure.Security.Authentication;
using Havit.Bonusario.Model;
using Havit.Bonusario.Primitives;
using Havit.Data.Patterns.UnitOfWorks;
using Havit.Extensions.DependencyInjection.Abstractions;
using Microsoft.AspNetCore.Authorization;

namespace Havit.Bonusario.Facades;

[Service]
[Authorize]

public class EmployeeFacade : IEmployeeFacade
{
	private readonly IEmployeeRepository employeeRepository;
	private readonly IApplicationAuthenticationService applicationAuthenticationService;
	private readonly IUnitOfWork unitOfWork;

	public EmployeeFacade(IEmployeeRepository employeeRepository,
		IApplicationAuthenticationService applicationAuthenticationService,
		IUnitOfWork unitOfWork)
	{
		this.employeeRepository = employeeRepository;
		this.applicationAuthenticationService = applicationAuthenticationService;
		this.unitOfWork = unitOfWork;
	}

	public async Task<Dto<EntryVisibility>> GetCurrentEmployeeDefaultEntryVisibility(CancellationToken cancellationToken = default)
	{
		var currentEmployee = await applicationAuthenticationService.GetCurrentEmployeeAsync(cancellationToken);
		return Dto.FromValue(currentEmployee.DefaultEntryVisibility);
	}

	public async Task UpdateCurrentEmployeeDefaultEntryVisibility(Dto<EntryVisibility> defaultVisibility, CancellationToken cancellationToken = default)
	{
		var currentEmployee = await applicationAuthenticationService.GetCurrentEmployeeAsync(cancellationToken);
		currentEmployee.DefaultEntryVisibility = defaultVisibility.Value;

		unitOfWork.AddForUpdate(currentEmployee);

		await unitOfWork.CommitAsync(cancellationToken);
	}

	public async Task<List<EmployeeReferenceDto>> GetAllEmployeeReferencesAsync(CancellationToken cancellationToken = default)
	{
		var employees = await employeeRepository.GetAllIncludingDeletedAsync(cancellationToken);
		return employees.Select(e => new EmployeeReferenceDto()
		{
			EmployeeId = e.Id,
			Name = e.Name,
			Email = e.Email,
			IsDeleted = e.Deleted is not null,
			DefaultVisibility = e.DefaultEntryVisibility
		}).ToList();
	}
}

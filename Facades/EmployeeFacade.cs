using Havit.Bonusario.Contracts;
using Havit.Bonusario.DataLayer.Repositories;
using Havit.Bonusario.Facades.Infrastructure.Security.Authentication;
using Havit.Bonusario.Model;
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

	public EmployeeFacade(IEmployeeRepository employeeRepository,
		IApplicationAuthenticationService applicationAuthenticationService)
	{
		this.employeeRepository = employeeRepository;
		this.applicationAuthenticationService = applicationAuthenticationService;
	}

	public async Task<Dto<int>> GetCurrentEmployeeId(CancellationToken cancellationToken = default)
	{
		var currentEmployee = await applicationAuthenticationService.GetCurrentEmployeeAsync(cancellationToken);
		return Dto.FromValue(currentEmployee.Id);
	}

	public async Task<List<EmployeeReferenceDto>> GetAllEmployeeReferencesAsync(CancellationToken cancellationToken = default)
	{
		var employees = await employeeRepository.GetAllIncludingDeletedAsync(cancellationToken);
		return employees.Select(e => new EmployeeReferenceDto()
		{
			EmployeeId = e.Id,
			Name = e.Name,
			Email = e.Email,
			IsDeleted = e.Deleted is not null
		}).ToList();
	}
}

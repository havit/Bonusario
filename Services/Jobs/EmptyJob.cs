using Havit.Extensions.DependencyInjection.Abstractions;
using Microsoft.Extensions.Logging;

namespace Havit.Bonusario.Services.Jobs;

[Service(Profile = Jobs.ProfileName)]
public class EmptyJob : IEmptyJob
{
	private readonly ILogger<EmptyJob> logger;

	public EmptyJob(ILogger<EmptyJob> logger)
	{
		this.logger = logger;
	}

	public async Task ExecuteAsync(CancellationToken cancellationToken)
	{
		logger.LogInformation("Begin: EmptyJob");

		await Task.Delay(1, cancellationToken);

		logger.LogInformation("End: EmptyJob");
	}
}

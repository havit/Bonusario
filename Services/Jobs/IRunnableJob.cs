using System.Threading;
using System.Threading.Tasks;

namespace Havit.Bonusario.Services.Jobs
{
	public interface IRunnableJob
	{
		Task ExecuteAsync(CancellationToken cancellationToken);
	}
}

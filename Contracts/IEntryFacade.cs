using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Havit.ComponentModel;

namespace Havit.Bonusario.Contracts
{
	[ApiContract]
	public interface IEntryFacade
	{
		Task<Dto<int>> GetMyRemainingPoints(Dto<int> periodId, CancellationToken cancellationToken = default);
		Task<List<EntryDto>> GetMyEntriesAsync(Dto<int> periodId, CancellationToken cancellationToken = default);
		Task DeleteEntryAsync(Dto<int> entryId, CancellationToken cancellationToken = default);
		Task<Dto<int>> CreateEntryAsync(EntryDto newEntry, CancellationToken cancellationToken = default);
	}
}

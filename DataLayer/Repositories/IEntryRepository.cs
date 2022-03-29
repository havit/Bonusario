using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.Data.Patterns.Repositories;
using Havit.Bonusario.Model;
using System.Threading;
using Havit.Bonusario.Contracts;

namespace Havit.Bonusario.DataLayer.Repositories;

public partial interface IEntryRepository
{
	Task<List<Entry>> GetEntriesCreatedByAsync(int periodId, int createdByEmployeeId, CancellationToken cancellationToken = default);
	Task<List<Entry>> GetEntriesReceivedByAsync(int periodId, int receivedByEmployeeId, CancellationToken cancellationToken = default);
	Task<int> GetPointsAssignedSumAsync(int periodId, int createdByEmployeeId, CancellationToken cancellationToken = default);
	Task<List<ResultItemDto>> GetResultsAsync(int periodId, CancellationToken cancellationToken = default);
	Task<List<ResultItemDto>> GetAggregateResultsAsync(int periodSetId, CancellationToken cancellationToken = default);
}

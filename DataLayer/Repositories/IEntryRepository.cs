using Havit.Bonusario.Model;
using Havit.Bonusario.Contracts;

namespace Havit.Bonusario.DataLayer.Repositories;

public partial interface IEntryRepository
{
	Task<List<Entry>> GetEntriesCreatedByAsync(int periodId, int createdByEmployeeId, CancellationToken cancellationToken = default);
	Task<List<Entry>> GetEntriesReceivedByAsync(int periodId, int receivedByEmployeeId, CancellationToken cancellationToken = default);
	Task<int> GetPointsAssignedSumAsync(int periodId, int createdByEmployeeId, CancellationToken cancellationToken = default);
	Task<List<Entry>> GetPublicReceivedEntriesAsync(int periodId, CancellationToken cancellationToken = default);
	/// <summary>
	/// Finds all entries in the specified period. Doesn't load entries' references (tags).
	/// </summary>
	/// <returns>A list of entries in the selected period.</returns>
	Task<List<Entry>> GetEntriesOfPeriod(int periodId, CancellationToken cancellationToken = default);
	Task<List<ResultItemDto>> GetResultsAsync(int periodId, CancellationToken cancellationToken = default);
	Task<List<ResultItemDto>> GetAggregateResultsAsync(int periodSetId, CancellationToken cancellationToken = default);
}

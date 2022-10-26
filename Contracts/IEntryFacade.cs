using Havit.ComponentModel;

namespace Havit.Bonusario.Contracts;

[ApiContract]
public interface IEntryFacade
{
	Task<Dto<int>> GetMyRemainingPoints(Dto<int> periodId, CancellationToken cancellationToken = default);
	Task<List<EntryDto>> GetMyGivenEntriesAsync(Dto<int> periodId, CancellationToken cancellationToken = default);
	Task<List<EntryDto>> GetMyReceivedEntriesAsync(Dto<int> periodId, CancellationToken cancellationToken = default);
	Task<List<EntryDto>> GetAllPublicReceivedEntries(Dto<int> periodId, CancellationToken cancellationToken = default);
	Task DeleteEntryAsync(Dto<int> entryId, CancellationToken cancellationToken = default);
	Task<Dto<int>> CreateEntryAsync(EntryDto newEntry, CancellationToken cancellationToken = default);
	Task<List<EntryDto>> GetEntriesOfPeriod(Dto<int> periodId, CancellationToken cancellationToken = default);
	Task<List<ResultItemDto>> GetResultsAsync(Dto<int> periodId, CancellationToken cancellationToken = default);
	Task<List<ResultItemDto>> GetAggregateResultsAsync(Dto<int> periodSetId, CancellationToken cancellationToken = default);
	Task UpdateEntryAsync(EntryDto entryDto, CancellationToken cancellationToken = default);
	Task SubmitEntriesAsync(List<int> entryDtos, CancellationToken cancellationToken = default);
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.Data.EntityFrameworkCore;
using Havit.Data.EntityFrameworkCore.Patterns.Repositories;
using Havit.Data.EntityFrameworkCore.Patterns.SoftDeletes;
using Havit.Data.Patterns.DataEntries;
using Havit.Data.Patterns.DataLoaders;
using Havit.Bonusario.Model;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Havit.Bonusario.Contracts;

namespace Havit.Bonusario.DataLayer.Repositories
{
	public partial class EntryDbRepository : IEntryRepository
	{
		public Task<List<Entry>> GetEntriesCreatedByAsync(int periodId, int createdByEmployeeId, CancellationToken cancellationToken = default)
		{
			return Data
				.Where(e => e.PeriodId == periodId)
				.Where(e => e.CreatedById == createdByEmployeeId)
				.Include(GetLoadReferences)
				.ToListAsync(cancellationToken);
		}

		public Task<List<Entry>> GetEntriesReceivedByAsync(int periodId, int receivedByEmployeeId, CancellationToken cancellationToken = default)
		{
			return Data
				.Where(e => e.PeriodId == periodId)
				.Where(e => e.RecipientId == receivedByEmployeeId)
				.Where(e => e.Submitted != null)
				.Include(GetLoadReferences)
				.ToListAsync(cancellationToken);
		}

		public Task<int> GetPointsAssignedSumAsync(int periodId, int createdByEmployeeId, CancellationToken cancellationToken = default)
		{
			return Data
				.Where(e => e.PeriodId == periodId)
				.Where(e => e.CreatedById == createdByEmployeeId)
				.SumAsync(e => e.Value);
		}

		public Task<List<ResultItemDto>> GetResultsAsync(int periodId, CancellationToken cancellationToken = default)
		{
			return Data
				.Where(e => e.PeriodId == periodId)
				.Where(e => e.Submitted != null)
				.GroupBy(e => e.RecipientId)
				.Select(g => new ResultItemDto() { RecipientId = g.Key, ValueSum = g.Sum(e => e.Value) })
				.ToListAsync(cancellationToken);
		}

		public Task<List<ResultItemDto>> GetAggregateResultsAsync(int periodSetId, CancellationToken cancellationToken = default)
		{
			return Data
				.Where(e => e.Period.PeriodSetId == periodSetId)
				.Where(e => e.Submitted != null)
				.GroupBy(e => e.RecipientId)
				.Select(g => new ResultItemDto() { RecipientId = g.Key, ValueSum = g.Sum(e => e.Value) })
				.ToListAsync(cancellationToken);
		}

		protected override IEnumerable<Expression<Func<Entry, object>>> GetLoadReferences()
		{
			yield return e => e.Tags;
		}
	}
}
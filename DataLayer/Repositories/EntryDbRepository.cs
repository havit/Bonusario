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

namespace Havit.Bonusario.DataLayer.Repositories
{
	public partial class EntryDbRepository : IEntryRepository
	{
		public Task<List<Entry>> GetEntriesCreatedByAsync(int createdByEmployeeId, CancellationToken cancellationToken = default)
		{
			return Data
				.Where(e => e.CreatedById == createdByEmployeeId)
				.Include(GetLoadReferences)
				.ToListAsync(cancellationToken);
		}

		protected override IEnumerable<Expression<Func<Entry, object>>> GetLoadReferences()
		{
			yield return e => e.Tags;
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.Bonusario.Contracts;
using Havit.Bonusario.Model;
using Havit.Extensions.DependencyInjection.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Havit.Bonusario.Services
{
	[Service(Lifetime = ServiceLifetime.Singleton)]
	public class EntryMapper : IEntryMapper
	{
		public EntryDto MapToEntryDto(Entry entry)
		{
			return new EntryDto()
			{
				Id = entry.Id,
				Text = entry.Text,
				CreatedById = entry.CreatedById,
				RecipientId = entry.RecipientId,
				Submitted = entry.Submitted,
				Value = entry.Value,
				Tags = entry.Tags.Select(et => et.Tag).ToList()
			};
		}
	}
}

using Havit.Bonusario.Contracts;
using Havit.Bonusario.Model;

namespace Havit.Bonusario.Services
{
	public interface IEntryMapper
	{
		EntryDto MapToEntryDto(Entry entry);
	}
}
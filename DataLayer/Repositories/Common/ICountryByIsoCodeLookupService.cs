
using Havit.Bonusario.Model.Common;

namespace Havit.Bonusario.DataLayer.Repositories.Common
{
	public interface ICountryByIsoCodeLookupService
	{
		Country GetCountryByIsoCode(string isoCode);
	}
}
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using Havit.ComponentModel;

namespace Havit.Bonusario.Contracts.System
{
	[ApiContract]
	public interface IDataSeedFacade
	{
		Task SeedDataProfile(string profileName);

		Task<Dto<string[]>> GetDataSeedProfiles();
	}
}
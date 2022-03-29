using Havit.ComponentModel;

namespace Havit.Bonusario.Contracts.Infrastructure;

[ApiContract]
public interface IDataSeedFacade
{
	Task SeedDataProfile(string profileName);

	Task<Dto<string[]>> GetDataSeedProfiles();
}

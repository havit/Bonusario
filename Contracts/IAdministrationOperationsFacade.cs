using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.ComponentModel;

namespace Havit.Bonusario.Contracts;

[ApiContract]
public interface IAdministrationOperationsFacade
{
	Task SubmitEntriesAndOpenNewPeriod(CancellationToken cancellationToken = default);
}

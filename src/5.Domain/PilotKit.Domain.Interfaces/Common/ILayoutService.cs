using PilotKit.Domain.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PilotKit.Domain.Interfaces.Common
{
    public interface ILayoutService : IDomainService
    {
        IList<PilotKitMenu> GetPilotKitMenu(string category);
    }
}

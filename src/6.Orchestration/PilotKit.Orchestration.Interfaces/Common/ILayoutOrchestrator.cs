using PilotKit.Domain.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PilotKit.Orchestration.Interfaces.Common
{
    public interface ILayoutOrchestrator : IOrchestrator
    {
        IList<PilotKitMenu> GetPilotKitMenu(string category);
    }
}

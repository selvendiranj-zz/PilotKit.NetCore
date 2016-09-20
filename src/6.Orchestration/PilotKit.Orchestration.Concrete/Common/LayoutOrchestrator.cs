using PilotKit.Domain.Interfaces.Common;
using PilotKit.Domain.Model.Common;
using PilotKit.Orchestration.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PilotKit.Orchestration.Concrete.Common
{
    public class LayoutOrchestrator : OrchestratorBase, ILayoutOrchestrator
    {
        private ILayoutService service;

        public LayoutOrchestrator(ILayoutService service)
        {
            this.service = service;
        }

        public IList<PilotKitMenu> GetPilotKitMenu(string category)
        {
            return this.service.GetPilotKitMenu(category);
        }
    }
}

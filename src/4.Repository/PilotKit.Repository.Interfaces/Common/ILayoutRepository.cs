using PilotKit.Domain.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PilotKit.Repository.Interfaces.Common
{
    public interface ILayoutRepository : IRepository
    {
        IList<PilotKitMenu> GetPilotKitMenu(string category);
        IList<PilotKitMenu> GetPilotKitMenu(long ParentId);
    }
}

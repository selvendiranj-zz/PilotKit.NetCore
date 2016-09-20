using PilotKit.Repository.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Insight.Database;
using System.Data;
using PilotKit.Domain.Model.Common;

namespace PilotKit.Repository.DataAccess.Common
{
    public abstract class LayoutRepository : RepositoryBase, ILayoutRepository
    {
        private const string QUERY_GetSubPilotKitMenu = "Select * From PilotKitMenu Where ParentId = @parentId";

        public abstract IDbConnection GetConnection();

        [Sql("Select * From PilotKitMenu Where Category = @Category And ParentId = 0")]
        public abstract IList<PilotKitMenu> GetPilotKitMenu(string category);

        public IList<PilotKitMenu> GetPilotKitMenu(long parentId)
        {
            return this.GetConnection().QuerySql<PilotKitMenu>(QUERY_GetSubPilotKitMenu, new { ParentId = parentId });
        }

    }
}

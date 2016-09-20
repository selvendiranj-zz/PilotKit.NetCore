using Autofac;
using PilotKit.Repository.DataAccess.Common;
using PilotKit.Repository.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Insight.Database;
using PilotKit.Infrastructure.CrossCutting.Constants;
using PilotKit.Repository.Interfaces.RevenueLoss;
using PilotKit.Repository.DataAccess.RevenueLoss;

namespace PilotKit.Repository.DataAccess
{
    public class AutofacModule : Autofac.Module
    {
        private string connectionString;

        /// <summary>
        ///     Method for getting sql connection
        /// </summary>
        /// <returns>
        ///     The <see cref="IDbConnection" />.
        /// </returns>
        private IDbConnection GetSqlConnection()
        {
            connectionString = ConnectionStrings.PilotKit;
            //return new GlimpseDbConnection(new SqlConnection(connectionString));
            return new SqlConnection(connectionString);
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            SqlInsightDbProvider.RegisterProvider();
            //GlimpseInsightDbProvider.RegisterProvider();

            IDbConnection connection = this.GetSqlConnection();

            //builder.RegisterInstance(connection.AsParallel<IRevenueLossRepository>());
            builder.Register(c => connection.AsParallel<RevenueLossRepository>()).As<IRevenueLossRepository>();
            builder.Register(c => connection.AsParallel<LayoutRepository>()).As<ILayoutRepository>();
            //builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(RepositoryBase))).Where(t => t.IsClass && t.GetInterfaces().Where(i => i.IsAssignableFrom(typeof(IRepository))).Count() > 0).AsImplementedInterfaces();

        }
    }
}

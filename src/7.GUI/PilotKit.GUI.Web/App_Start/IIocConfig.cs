using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PilotKit.Web.App_Start
{
    public interface IIocConfig
    {
        ContainerBuilder RegisterDependencies(ContainerBuilder builder);
        void InitializeAppConstants();
    }
}

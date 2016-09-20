using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using PilotKit.Orchestration.Concrete.RevenueLoss;
using PilotKit.Orchestration.Interfaces.RevenueLoss;
using System.Reflection;
using PilotKit.Orchestration.Concrete.Common;
using PilotKit.Orchestration.Interfaces.Common;

namespace PilotKit.Orchestration.Concrete
{
    public class AutofacModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            //builder.RegisterType<RevenueLossOrchestrator>().As<IRevenueLossOrchestrator>();
            //builder.RegisterType<LayoutOrchestrator>().As<ILayoutOrchestrator>();
            builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(OrchestratorBase))).Where(t => t.IsClass && t.GetInterfaces().Where(i => i.IsAssignableFrom(typeof(IOrchestrator))).Count() > 0).AsImplementedInterfaces();
        }
    }
}

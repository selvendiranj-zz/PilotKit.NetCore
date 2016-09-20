using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using System.Reflection;
using PilotKit.Infrastructure.CrossCutting.Common;
using PilotKit.Infrastructure.Interfaces.Common;

namespace PilotKit.Infrastructure.CrossCutting
{
    public class AutofacModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(InfrastructureBase))).Where(t => t.IsClass && t.GetInterfaces().Where(i => i.IsAssignableFrom(typeof(IInfrastructure))).Count() > 0).AsImplementedInterfaces();
        }
    }
}

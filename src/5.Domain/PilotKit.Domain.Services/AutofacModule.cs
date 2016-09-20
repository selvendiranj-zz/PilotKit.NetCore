using Autofac;
using PilotKit.Domain.Interfaces.Common;
using PilotKit.Domain.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace PilotKit.Domain.Services
{
    public class AutofacModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(DomainServiceBase))).Where(t => t.IsClass && t.GetInterfaces().Where(i => i.IsAssignableFrom(typeof(IDomainService))).Count() > 0).AsImplementedInterfaces();
        }
    }
}

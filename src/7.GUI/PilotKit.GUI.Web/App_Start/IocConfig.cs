using Autofac;
using Autofac.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime;
using System.Reflection;
//using System.Runtime.Loader;
using System.Threading.Tasks;
//using Microsoft.Dnx.Runtime;
using PilotKit.Infrastructure.CrossCutting.Constants;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.Extensions.Configuration;
using Autofac.Configuration;
using PilotKit.Infrastructure.CrossCutting.Utilities;
using Microsoft.Extensions.Options;

namespace PilotKit.Web.App_Start
{
    public class IocConfig : IIocConfig //,AssemblyLoadContext
    {
        private AppSettings appSettings;
        private ConnectionStrings connectionStrings;
        //private IAssemblyLoadContextAccessor acccessor;

        public IocConfig(IOptions<AppSettings> appSettings, IOptions<ConnectionStrings> connectionStrings) // IAssemblyLoadContextAccessor acccessor
        {
            this.appSettings = appSettings.Value;
            this.connectionStrings = connectionStrings.Value;
            // this.acccessor = acccessor;

            //AssemblyLoadContext.InitializeDefaultContext(this);

            //this.InitializeAppConstants();
        }

        public ContainerBuilder RegisterDependencies(ContainerBuilder builder)
        {
            var path = AppSettings.PackageLocalPath +
                ((AppSettings.PackageLocalPath.Contains(".dnx")) ? @"\" + AppSettings.PackageSearchPattern : @"\");
            var searchPattern = AppSettings.PackageSearchPattern + "*.dll";

            // The commented assemblies are already loaded into AppDomain from project.json
            //LoadAssemblyToAppDomain(path + "Infrastructure.Interfaces", searchPattern);
            //LoadAssemblyToAppDomain(path + "Infrastructure.CrossCutting", searchPattern);
            ////LoadAssemblyToAppDomain(path + "Repository.Interfaces", searchPattern);
            ////LoadAssemblyToAppDomain(path + "Repository.DataAccess", searchPattern);
            //LoadAssemblyToAppDomain(path + "Domain.Model", searchPattern);
            ////LoadAssemblyToAppDomain(path + "Domain.Interfaces", searchPattern);
            ////LoadAssemblyToAppDomain(path + "Domain.Services", searchPattern);
            //LoadAssemblyToAppDomain(path + "Orchestration.Interfaces", searchPattern);
            ////LoadAssemblyToAppDomain(path + "Orchestration.Concrete", searchPattern);
            LoadAssemblyToAppDomain(path, searchPattern);
            //RegisterAutofacModules(builder, path + "Infrastructure.CrossCutting", searchPattern);
            //RegisterAutofacModules(builder, path + "Repository.DataAccess", searchPattern);
            //RegisterAutofacModules(builder, path + "Domain.Services", searchPattern);
            //RegisterAutofacModules(builder, path + "Orchestration.Concrete", searchPattern);

            RegisterAutofacModules(builder, AppSettings.PackageSearchPattern);

            //var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
            //var refAssemblies = loadedAssemblies.SelectMany(x => x.GetReferencedAssemblies()).Distinct()
            //                    .Where(y => loadedAssemblies.Any(a => a.FullName == y.FullName) == false)
            //                    .Where(y => !y.FullName.StartsWith("System.") && !y.FullName.StartsWith("Microsoft."))
            //                    .ToList();
            //refAssemblies.ForEach(x => loadedAssemblies.Add(AppDomain.CurrentDomain.Load(x)));

            return builder;
        }

        private void LoadAssemblyToAppDomain(string path, string searchPattern)
        {
            if (String.IsNullOrWhiteSpace(path))
            {
                return;
            }
            // Gets all compiled assemblies.
            var assemblies = Directory.GetFiles(path, searchPattern, SearchOption.AllDirectories)
                                      .Select(Assembly.LoadFrom);

            foreach (var assembly in assemblies)
            {
                System.AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(assembly.Location));
            }
        }

        private ContainerBuilder RegisterAutofacModules(ContainerBuilder builder, string searchPattern)
        {
            //if (String.IsNullOrWhiteSpace(path))
            //{
            //    return null;
            //}
            // Gets all compiled assemblies.
            // This is particularly useful when extending applications functionality from 3rd parties,
            // if there are interfaces within the modules.
            //var assemblies = Directory.GetFiles(path, searchPattern, SearchOption.AllDirectories)
            //                          .Select(Assembly.LoadFrom);

            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();

            var assemblies = loadedAssemblies//.SelectMany(x => x.GetReferencedAssemblies()).Distinct()
                                             //.Where(y => loadedAssemblies.Any(a => a.FullName == y.FullName) == false)
                             .Where(y => y.FullName.StartsWith(searchPattern))
                             .ToList()
                             .Distinct();

            //assemblies.ForEach(x => loadedAssemblies.Add(AppDomain.CurrentDomain.Load(x)));

            foreach (var assembly in assemblies)
            {
                // Gets the all modules from each assembly to be registered.
                // Make sure that each module **MUST** have a parameterless constructor.
                var modules = assembly.GetTypes()
                                      .Where(p => typeof(IModule).IsAssignableFrom(p) && !p.IsAbstract)
                                      .Select(p => (IModule)Activator.CreateInstance(p));

                // Regsiters each module.
                foreach (var module in modules)
                {
                    builder.RegisterModule(module);
                }
            }

            return builder;
        }

        //protected Assembly Load(AssemblyName assemblyName)
        //{
        //    var assemblyPath = "" + assemblyName + ".dll";

        //    //return System.AppDomain.CurrentDomain.Load(assemblyPath);
        //    //Microsoft.Dnx.Runtime.Loader.LoadContext.LoadFile(assemblyPath);

        //    //return base.LoadFromAssemblyPath(assemblyPath);
        //}

        private ContainerBuilder RegisterAutofacModulesFromConfig(ContainerBuilder builder)
        {
            //var assembly = acccessor.Default.Load("PilotKit.Repository.DataAccess");

            var path = AppSettings.PackageLocalPath + @"\";
            var searchPattern = AppSettings.PackageSearchPattern;
            //var subPath = @"\Debug\net451";

            //Assembly.Load(new AssemblyName(path + "Infrastructure.Interfaces" + subPath));
            //Assembly.Load(new AssemblyName(path + "Infrastructure.CrossCutting" + subPath));
            //Assembly.Load(new AssemblyName(path + "Repository.Interfaces" + subPath));
            //Assembly.Load(new AssemblyName(path + "Repository.DataAccess" + subPath));
            //Assembly.Load(new AssemblyName(path + "Domain.Model" + subPath));
            //Assembly.Load(new AssemblyName(path + "Domain.Interfaces" + subPath));
            //Assembly.Load(new AssemblyName(path + "Domain.Services" + subPath));
            //Assembly.Load(new AssemblyName(path + "Orchestration.Interfaces" + subPath));
            //Assembly.Load(new AssemblyName(path + "Infrastructure.Concrete" + subPath));

            Directory.GetFiles(path, searchPattern, SearchOption.AllDirectories)
                                      .Select(Assembly.LoadFrom);

            // Add the configuration to the ConfigurationBuilder.
            var config = new ConfigurationBuilder();
            config.AddJsonFile("autofac.json");

            // Register the ConfigurationModule with Autofac.
            var module = new ConfigurationModule(config.Build());
            builder.RegisterModule(module);

            return builder;
        }

        public void InitializeAppConstants()
        {
            RevLossConstants.CurrentAssesmentYear = DateTime.Now.Year;
            AppSettings.ExcelStageLocation = Environment.ExpandEnvironmentVariables(AppSettings.ExcelStageLocation);
            AppSettings.RevenueLossReportPath = Environment.ExpandEnvironmentVariables(AppSettings.RevenueLossReportPath);
        }
    }
}

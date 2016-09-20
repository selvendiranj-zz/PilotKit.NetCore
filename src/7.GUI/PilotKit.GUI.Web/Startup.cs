using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PilotKit.GUI.Web.Data;
using PilotKit.GUI.Web.Models;
using PilotKit.GUI.Web.Services;
using Autofac;
using PilotKit.Web.App_Start;
using Microsoft.Extensions.PlatformAbstractions;
using PilotKit.Infrastructure.CrossCutting.Constants;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Authorization;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace PilotKit.GUI.Web
{
    public class Startup
    {
        private string staticViewPath = "";
        public IConfigurationRoot Configuration { get; set; }
        private IContainer Container { get; set; }
        private IIocConfig AutofacConfig { get; set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("hosting.json", optional: true)
                .AddJsonFile("config.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"config.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();

                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Add Glimpse
            // services.AddGlimpse();

            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("PilotKit")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc();

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();

            // only allow authenticated users
            var defaultPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

            services.AddMvc(setup =>
            {
                setup.Filters.Add(new AuthorizeFilter(defaultPolicy));
            });

            // Add MVC services to the services container.
            //services.AddMvc();
            //services.AddMvc().AddJsonOptions(options =>
            //{
            //    options.SerializerSettings.ContractResolver =
            //        new CamelCasePropertyNamesContractResolver();
            //});

            // Add Application settings to the services container.
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.Configure<ConnectionStrings>(Configuration.GetSection("ConnectionStrings"));
            services.AddTransient<IIocConfig, IocConfig>();

            // Create a provider from the service collection
            IServiceProvider provider = services.BuildServiceProvider();

            // Instantiate Autofac ModuleLoading class
            this.AutofacConfig = provider.GetService<IIocConfig>();

            //ConfigManager.AppSettings = ;
            //ConfigManager.ConnectionStrings = ;

            // Create the container builder.
            var builder = new ContainerBuilder();
            this.AutofacConfig.RegisterDependencies(builder);

            // Register dependencies, populate the services from
            // the collection, and build the container.
            // builder.RegisterType<MyType>().As<IMyType>();
            builder.Populate(services);
            Container = builder.Build();

            // Return the IServiceProvider resolved from the container.
            return Container.Resolve<IServiceProvider>();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="loggerFactory"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // Use Glimpse
            // app.UseGlimpse();

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseApplicationInsightsRequestTelemetry();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // For more details on creating database during deployment see http://go.microsoft.com/fwlink/?LinkID=615859
                try
                {
                    using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
                    {
                        serviceScope.ServiceProvider.GetService<ApplicationDbContext>().Database.Migrate();
                    }
                }
                catch { }
            }

            app.ApplicationServices.GetRequiredService<IIocConfig>().InitializeAppConstants();
            
            app.UseApplicationInsightsExceptionTelemetry();

            //app.UseStaticFiles();
            staticViewPath = AppSettings.StaticViewPath;
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(staticViewPath),
                //RequestPath = new PathString("/StaticFiles")
            });

            app.UseIdentity();

            // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715
            
            // If we're self-hosting, enable integrated authentication (if we're using
            // IIS, this will be done at the IIS configuration level).
            //var listener = app.ServerFeatures.Get<WebListener>();
            //if (listener != null)
            //{
            //    listener.AuthenticationManager.AuthenticationSchemes = AuthenticationSchemes.NTLM;
            //}

            app.UseMvc(routes =>
            {
                // routes.MapRoute(name: "default", template: "{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute(name: "default", template: "{controller=Layout}/{action=Index}/{id?}");
                // routes.MapWebApiRoute("defaultApi", "api/{controller}/{action=Index}/{id?}");
                // 404 routing, or otherwise routing logic, if route is not found go Home
                // routes.MapRoute("spa-fallback", "{*anything}", new { controller = "Layout", action = "Index" });
            });
        }
    }
}

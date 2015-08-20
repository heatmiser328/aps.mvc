using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

using ica.aps.api.Controllers;
using ica.aps.data.db;
using ica.aps.data.interfaces;
using ica.aps.data.repositories;

namespace ica.aps.api
{    
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        private readonly IWindsorContainer container;

        public WebApiApplication() 
        {
            System.Configuration.ConnectionStringSettings cs = System.Configuration.ConfigurationManager.ConnectionStrings["aps"];

            this.container = new WindsorContainer()
                .Register(Component.For<IDatabase>()
                    .ImplementedBy<Database>()
                    .DependsOn(Dependency.OnValue("provider", cs.ProviderName))
                    .DependsOn(Dependency.OnValue("connectionString", cs.ConnectionString))
                )
                //.Register(Component.For<IUserRepository>()
                //    .ImplementedBy<UserRepository>()
                //)
                .Register(Component.For<IEmployeeRepository>()
                    .ImplementedBy<EmployeeRepository>()
                //.DependsOn(Dependency.OnComponent<IRentRepository, RentRepository>())
                )
                .Register(Component.For<IRentRepository>()
                    .ImplementedBy<RentRepository>()
                )

                //.Register(Component.For<EmployeeController>()
                    //.DependsOn(Dependency.OnComponent<IEmployeeRepository, EmployeeRepository>())
                //)
                ;
        }

        protected void Application_Start()        
        {
            GlobalConfiguration.Configuration.Services.Replace(
                typeof(IHttpControllerActivator),
                new WindsorCompositionRoot(this.container));

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
        }

        public override void Dispose()
        {
            this.container.Dispose();
            base.Dispose();
        }
    }
}
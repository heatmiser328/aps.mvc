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
using Castle.Windsor.Installer;

using ica.aps.api.Controllers;
using ica.aps.data.db;
using ica.aps.data.interfaces;
using ica.aps.data.repositories;
using ica.aps.core.interfaces;
using ica.aps.core.managers;

namespace ica.aps.api
{    
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        private readonly IWindsorContainer container;

        public WebApiApplication() 
        {            
            this.container = new WindsorContainer();
        }

        protected void Application_Start()                
        {
            try
            {
                /*             
                GlobalConfiguration.Configuration.Services.Replace(
                    typeof(IHttpControllerActivator),
                    new WindsorCompositionRoot(this.container));
                */
                //var container = new WindsorContainer();
                this.container.Install(FromAssembly.This());
                var dr = new CastleWindsor.DependencyResolver(this.container.Kernel);
                GlobalConfiguration.Configuration.DependencyResolver = dr;

                System.Configuration.ConnectionStringSettings cs = System.Configuration.ConfigurationManager.ConnectionStrings["aps"];
                this.container
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
                    .Register(Component.For<IDailyGrossRepository>()
                        .ImplementedBy<DailyGrossRepository>()
                    )

                    .Register(Component.For<IPayrollManager>()
                        .ImplementedBy<PayrollManager>()
                    )

                    /*
                    .Register(Component.For<PayrollController>()
                        .DependsOn(Dependency.OnComponent<IPayrollRepository, PayrollRepository>())
                    )

                    .Register(Component.For<EmployeeController>()
                        .DependsOn(Dependency.OnComponent<IEmployeeRepository, EmployeeRepository>())
                    )
                    */
                    ;

                AreaRegistration.RegisterAllAreas();

                WebApiConfig.Register(GlobalConfiguration.Configuration);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public override void Dispose()
        {
            this.container.Dispose();
            base.Dispose();
        }
    }
}
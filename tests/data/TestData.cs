using System;
using System.Data;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

using Xunit;
using Shouldly;

using ica.aps.data.interfaces;
using ica.aps.data.db;

namespace Data
{
    [Trait("category", "Data")]
    public class TestData
    {
        private IWindsorContainer _container;

        public TestData()
        {
            System.Configuration.ConnectionStringSettings cs = System.Configuration.ConfigurationManager.ConnectionStrings["aps"];

            _container = new WindsorContainer();
            _container
                .Register(Component.For<IDatabase>()
                    .ImplementedBy<Database>()
                    .DependsOn(Dependency.OnValue("provider", cs.ProviderName))
                    .DependsOn(Dependency.OnValue("connectionString", cs.ConnectionString))
                    //.DependsOn(Dependency.OnValue("provider", "System.Data.SqlClient"))
                    //.DependsOn(Dependency.OnValue("connectionString", "Data Source=dev-s01;Initial Catalog=aps;User ID=sa;Password=sql@dm1n"))
                );            
        }


        //[Fact]        
        public void Seed()
        {
            IDatabase db = _container.Resolve<IDatabase>();
            TestHelpers.TestData.Reset(db);
        }		
    }
}

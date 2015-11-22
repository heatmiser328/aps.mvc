using System;
using System.Collections.Generic;
using System.Linq;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

using Xunit;
using Shouldly;

using ica.aps.core;
using ica.aps.data.models;
using ica.aps.data.db;
using ica.aps.data.interfaces;
using ica.aps.data.repositories;
//using ica.aps.tests.mocks;

namespace Repositories
{ 
    public class DailyGrossRepositoryTest
    {
        private IWindsorContainer _container;
        private Employee _employee;
        private WorkWeek _ww;

        public DailyGrossRepositoryTest()
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
                )
                .Register(Component.For<IDailyGrossRepository>()
                    .ImplementedBy<DailyGrossRepository>()
                );

            var now = DateTime.Now;
            _ww = now.WorkWeek();
            IDatabase db = _container.Resolve<IDatabase>();
            TestHelpers.TestData.Reset(db);
            _employee = TestHelpers.TestData.GetEmployee(db, "Tom");
        }

        [Fact]
        public void Get()
        {            
        	var dgr = _container.Resolve<IDailyGrossRepository>();
            var grosses = dgr.Get(_employee, _ww.Start, _ww.End);
            grosses.ShouldNotBe(null);
            grosses.ShouldNotBeEmpty();
            grosses.Count().ShouldBe(6);
			
			var gross = grosses.First();
            gross.ShouldNotBe(null);
            //gross.GrossPay.ShouldBe()
            gross.GrossTDS.ShouldBe(_ww.Start);
        }
    }
}
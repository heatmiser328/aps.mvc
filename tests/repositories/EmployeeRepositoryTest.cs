using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

using Xunit;
using Shouldly;

using ica.aps.data.models;
using ica.aps.data.db;
using ica.aps.data.interfaces;
using ica.aps.data.repositories;
//using ica.aps.tests.mocks;

namespace Repositories
{
    public class EmployeeRepositoryTest
    {
        private IWindsorContainer _container;

        public EmployeeRepositoryTest()
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
                .Register(Component.For<IRentRepository>()
                    .ImplementedBy<RentRepository>()
                )
                .Register(Component.For<IEmployeeRepository>()
                    .ImplementedBy<EmployeeRepository>()
                );

            IDatabase db = _container.Resolve<IDatabase>();
            TestHelpers.TestData.ResetBlank(db);
        }

        [Fact]
        public void Get()
        {
            var er = _container.Resolve<IEmployeeRepository>();
            var employees = er.Get();
            employees.ShouldNotBe(null);
            employees.ShouldNotBeEmpty();
            employees.Count().ShouldBe(6);

            var employee = employees.First();
            employee.ShouldNotBe(null);
            employee.FirstName.ShouldBe("Tom");
            employee.LastName.ShouldBe("Capuano");
            employee.Title.ShouldBe("Owner");
            employee.Enabled.ShouldBe(true);
            employee.EffectiveRent().RentPct.ShouldBe(0.83M);
        }
    }
}
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

using Mocks;

namespace Repositories
{
    [Trait("category", "Repositories")]
    public class DailyGrossRepositoryTest
    {
        IWindsorContainer _container;
        IDatabase _db;
        Employee _employee;        
        DateTime _now;
        WorkWeek _ww;
        IDailyGrossRepository _repos;

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

            _now = DateTime.Now;
            _ww = _now.WorkWeek();
            _db = _container.Resolve<IDatabase>();
            TestHelpers.TestData.Reset(_db);
            _employee = TestHelpers.TestData.GetEmployee(_db, "Tom");            

            _repos = _container.Resolve<IDailyGrossRepository>();
        }

        [Fact]
        public void Get()
        {                    	
            var grosses = _repos.Get(_employee, _ww.Start, _ww.End);
            grosses.ShouldNotBe(null);
            grosses.ShouldNotBeEmpty();
            grosses.Count().ShouldBe(6);
			
			var gross = grosses.First();
            gross.ShouldNotBe(null);
            //gross.GrossPay.ShouldBe()
            gross.GrossTDS.ShouldBe(_ww.Start);
        }

        [Fact]
        public void Insert()
        {            
            var dt = _ww.Start.AddDays(7);
            var ww = dt.WorkWeek();
            var gross = MockData.MockGross(ww.Start);

            _repos.Insert(_employee, gross);

            //var grosses = _repos.Get(_employee, ww.Start, ww.End);
            var grosses = TestHelpers.TestData.GetDailyGrosses(_db, _employee, ww.Start, ww.End);
            grosses.ShouldNotBe(null);
            grosses.ShouldNotBeEmpty();
            grosses.Count().ShouldBe(1);

            var actual = grosses.First();
            actual.ShouldNotBe(null);
            actual.Gross.ShouldBe(gross.Gross);
            actual.GrossTDS.ShouldBe(gross.GrossTDS);
        }

        [Fact]
        public void Update()
        {
            var expected = TestHelpers.TestData.GetDailyGrosses(_db, _employee).First();
            expected.Gross = expected.Gross + MockData.RandomDecimal(1, 100);
            expected.GrossTDS = DateTime.Now;

            _repos.Update(_employee, expected);
            var actual = TestHelpers.TestData.GetDailyGross(_db, expected.DailyGrossID.Value);
            actual.ShouldNotBe(null);
            actual.DailyGrossID.ShouldBe(expected.DailyGrossID);
            actual.Gross.ShouldBe(expected.Gross);
            //actual.GrossTDS.ShouldBe(expected.GrossTDS);
            actual.GrossTDS.Year.ShouldBe(expected.GrossTDS.Year);
            actual.GrossTDS.Month.ShouldBe(expected.GrossTDS.Month);
            actual.GrossTDS.Day.ShouldBe(expected.GrossTDS.Day);
            actual.GrossTDS.Hour.ShouldBe(expected.GrossTDS.Hour);
            actual.GrossTDS.Minute.ShouldBe(expected.GrossTDS.Minute);
        }
    }
}

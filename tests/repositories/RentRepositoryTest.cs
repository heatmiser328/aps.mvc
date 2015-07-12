using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

using Xunit;
using Shouldly;

using ica.aps.core.interfaces;
using ica.aps.core.models;
using ica.aps.data.db;
using ica.aps.data.interfaces;
using ica.aps.data.repositories;
//using ica.aps.tests.mocks;

namespace Repositories
{ 
    public class RentRepositoryTest
    {
        private IWindsorContainer _container;
        private Guid _employeeID;

        public RentRepositoryTest()
        {
            _container = new WindsorContainer();
            _container
                .Register(Component.For<IDatabase>()
                    .ImplementedBy<Database>()
                    .DependsOn(Dependency.OnValue("provider", "System.Data.SqlClient"))
                    .DependsOn(Dependency.OnValue("connectionString", "Data Source=dev-s01;Initial Catalog=aps;User ID=sa;Password=sql@dm1n"))
                )
                .Register(Component.For<IRentRepository>()
                    .ImplementedBy<RentRepository>()
                );

            IDatabase db = _container.Resolve<IDatabase>();
            using (IDbConnection conn = db.Create())
            {
                conn.Open();
                IDbCommand cmd = conn.CreateCommand();                
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT EmployeeID FROM Employee WHERE FirstName = 'Tom'";
                _employeeID = (Guid)cmd.ExecuteScalar();
            }
        }
        
        [Fact]
        public void GetRents_Default()
        {            
        	IRentRepository rr = _container.Resolve<IRentRepository>();
			IEmployee employee = new Employee {
                ID = _employeeID
			};
            IList<IRent> rents = rr.GetRents(employee);
            rents.ShouldNotBe(null);
            rents.ShouldNotBeEmpty();            
			
			IRent rent = rents.First();
            rent.ShouldNotBe(null);
            rent.RentPct.ShouldBe(0.83M);
            rent.EffectiveDate.ShouldBe(DateTime.Parse("2000-01-01 00:00:00.000"));
        }

		/*        
        public void GetRents_Connection()
        {
            IDBFactory dbf = _container.Create<IDBFactory>();
			using (IDbConnection conn = dbf.Create())
			{
                conn.Open();
				IRentRepository rr = new RentRepository(_container, conn);
				
				IEmployee employee = new Employee {
                    ID = _employeeID
				};
                IList<IRent> rents = rr.GetRents(employee);
				Assert.IsNotNull(rents);
				Assert.AreEqual(1, rents.Count);
				
				IRent rent = rents.First();
				Assert.AreEqual(0.83M, rent.RentPct);
				Assert.AreEqual(DateTime.Parse("2000-01-01 00:00:00.000"), rent.EffectiveDate);
			}
        }
		*/
    }
}
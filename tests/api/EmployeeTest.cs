using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using Castle.MicroKernel.Registration;
using Castle.Windsor;

using Xunit;
using Shouldly;
using NSubstitute;
using Mocks;

using ica.aps.api.Controllers;
using ica.aps.data.interfaces;
using ica.aps.data.repositories;

namespace api
{
    public class EmployeeTest
    {
        private IWindsorContainer _container;        
        private IEmployeeRepository _repos;

        public EmployeeTest()
        {
            _repos = Substitute.For<IEmployeeRepository>();
            _repos.GetEmployees().Returns(MockData.Employees);

            _container = new WindsorContainer();
            _container
                .Register(Component.For<IEmployeeRepository>()
                    .Instance(_repos)
                )
                .Register(Component.For<EmployeeController>()                    
                );
        }

        [Fact]
        public void GetEmployees()
        {
            var expected = MockData.Employees;
            var c = _container.Resolve<EmployeeController>();
            var actual = c.Get();
            actual.ShouldNotBe(null);
            actual.ShouldNotBeEmpty();
            actual.Count().ShouldBe(expected.Count);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

using Castle.MicroKernel.Registration;
using Castle.Windsor;

using Xunit;
using Shouldly;
using NSubstitute;
using Mocks;

using ica.aps.api.Controllers;
using ica.aps.core.interfaces;
using ica.aps.data.models;

namespace api
{
    [Trait("category", "Controllers")]
    public class PayrollControllerTest
    {
        IWindsorContainer _container;
        IPayrollManager _mgr;
        DateTime _now, _now_fail;
        PayrollController _controller;
        Payroll _payroll, _payroll_fail;

        public PayrollControllerTest()
        {
            _now = DateTime.Now;
            _now_fail = DateTime.Now.AddMinutes(-5);
            _payroll = MockData.Payroll;
            _payroll_fail = new Payroll();
            _mgr = Substitute.For<IPayrollManager>();
            _mgr.GetPayroll(_now).Returns(MockData.Payroll);
            _mgr.GetPayroll(_now_fail).Returns(x => { throw new Exception("Wth?"); });
            _mgr.When(x => x.SavePayroll(_payroll));
            _mgr.When(x => x.SavePayroll(_payroll_fail))
                .Do(x => { throw new Exception("Wth?"); });
            //_mgr.SavePayroll(_payroll);

            _container = new WindsorContainer();
            _container
                .Register(Component.For<IPayrollManager>()
                    .Instance(_mgr)
                )
                .Register(Component.For<PayrollController>()
                    .OnCreate((kernel, instance) => {
                        instance.Request = new System.Net.Http.HttpRequestMessage();
                        instance.Configuration = new System.Web.Http.HttpConfiguration();
                    })                            
                );

            _controller = _container.Resolve<PayrollController>();
        }

        [Fact]
        public void GetPayroll()
        {
            var expected = _payroll;
            var actual = _controller.Get(_now);
            _mgr.Received(1).GetPayroll(_now);
            actual.ShouldNotBe(null);
            actual.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
            actual.Content.ShouldNotBe(null);
            actual.Content.Headers.ContentType.MediaType.ShouldBe("application/json");
            //Payroll payroll;            
            //actual.TryGetContentValue<Payroll>(out payroll).ShouldBe(true);
            //payroll.ShouldBe(expected);
        }

        [Fact]
        public void GetPayroll_Error()
        {
            Should.Throw<HttpResponseException>(() => {
                _controller.Get(_now_fail);
            });                        
            _mgr.Received(1).GetPayroll(_now_fail);
        }

        [Fact]
        public void PostPayroll()
        {
            var expected = _payroll;
            var actual = _controller.Post(expected);
            _mgr.Received(1).SavePayroll(expected);
            actual.ShouldNotBe(null);
            actual.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);
            actual.Content.ShouldNotBe(null);
            actual.Content.Headers.ContentType.MediaType.ShouldBe("application/json");
        }

        [Fact]
        public void PostPayroll_Error()
        {
            var expected = _payroll_fail;
            Should.Throw<HttpResponseException>(() => {
                _controller.Post(expected);
            });                        
            _mgr.Received(1).SavePayroll(expected);
        }
    }
}

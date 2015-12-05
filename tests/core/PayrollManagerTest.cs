using System;
using System.Collections.Generic;
using System.Linq;

using Castle.MicroKernel.Registration;
using Castle.Windsor;

using Xunit;
using Shouldly;
using NSubstitute;
using Mocks;

using ica.aps.core;
using ica.aps.core.interfaces;
using ica.aps.core.managers;
using ica.aps.data.interfaces;
using ica.aps.data.models;

namespace Core
{ 
    [Trait("category", "Managers")]
    public class PayrollManagerTest
    {
        IWindsorContainer _container;
        IEmployeeRepository _emprepos;
        IDailyGrossRepository _dgrepos;
        IPayrollManager _mgr;
        DateTime _now;
        WorkWeek _ww;
        IEnumerable<Employee> _employees;
        IEnumerable<DailyGross> _grosses;

        public PayrollManagerTest()
        {
            _now = DateTime.Now;
            _ww = _now.WorkWeek();
            _employees = MockData.Employees;
            _grosses = MockData.Grosses(_ww.Start, _ww.End);

            _emprepos = Substitute.For<IEmployeeRepository>();
            _emprepos.Get().Returns(_employees);

            _dgrepos = Substitute.For<IDailyGrossRepository>();
            _dgrepos.Get(Arg.Any<Employee>(), Arg.Any<DateTime>(), Arg.Any<DateTime>()).ReturnsForAnyArgs(_grosses);
                
            _container = new WindsorContainer();
            _container
                .Register(Component.For<IEmployeeRepository>()
                    .Instance(_emprepos)
                )
                .Register(Component.For<IDailyGrossRepository>()
                    .Instance(_dgrepos)
                )

                .Register(Component.For<IPayrollManager>()
                    .ImplementedBy<PayrollManager>()
                );

            _mgr = _container.Resolve<IPayrollManager>();
        }

        [Fact]
        public void GetEmployees()
        {
            var expected = _employees;
            var employees = _mgr.GetEmployees();
            _emprepos.Received(1).Get();
            employees.ShouldNotBe(null);
            employees.ShouldNotBeEmpty();
            employees.ShouldBe(expected);
        }

        [Fact]
        public void GetPayroll()
        {
            var payroll = _mgr.GetPayroll(_now);
            _emprepos.Received(1).Get();
            _employees.Select(e => {
                _dgrepos.Received(1).Get(e, _ww.Start, _ww.End);
                return e;
            });
            payroll.ShouldNotBe(null);
            payroll.StartTDS.ShouldBe(_ww.Start);
            payroll.EndTDS.ShouldBe(_ww.End);
            payroll.Employees.ShouldNotBe(null);
            payroll.Employees.ShouldNotBeEmpty();
            payroll.Employees.Select(e => {
                _employees.ShouldContain(e.Employee);
                e.Grosses.ShouldNotBeEmpty();                
                e.Grosses.Select(g => {
                    _grosses.ShouldContain(g);
                    return g;
                });
                return e;
            });
        }


        [Fact]
        public void GetPayroll_New()
        {
            _now = _now.AddMonths(1);
            _ww = _now.WorkWeek();
            var payroll = _mgr.GetPayroll(_now);
            _emprepos.Received(1).Get();
            _employees.Select(e =>
            {
                _dgrepos.Received(1).Get(e, _ww.Start, _ww.End);
                return e;
            });
            payroll.ShouldNotBe(null);
            payroll.StartTDS.ShouldBe(_ww.Start);
            payroll.EndTDS.ShouldBe(_ww.End);
            payroll.Employees.ShouldNotBe(null);
            payroll.Employees.ShouldNotBeEmpty();
            payroll.Employees.Select(e =>
            {
                _employees.ShouldContain(e.Employee);
                e.Grosses.ShouldNotBeEmpty();
                DateTime dt = _ww.Start;
                e.Grosses.Select(g =>
                {
                    g.Gross.ShouldBe(0);
                    g.GrossTDS.ShouldBe(dt);
                    dt = dt.AddDays(1);
                    return g;
                });
                return e;
            });
        }

        [Fact]
        public void SavePayroll_Existing()
        {
            var payroll = MockData.Payroll;
            var employee = payroll.Employees.First();
            var gross = employee.Grosses.First();
            gross.Dirty = true;

            _mgr.SavePayroll(payroll);
            _emprepos.DidNotReceive().Get();
            _dgrepos.Received(1).Update(employee.Employee, gross);
            _dgrepos.DidNotReceive().Insert(Arg.Any<Employee>(), Arg.Any<DailyGross>());
        }

        [Fact]
        public void SavePayroll_New()
        {
            var payroll = MockData.Payroll;
            var employee = payroll.Employees.First();
            var gross = employee.Grosses.First();
            gross.DailyGrossID = null;
            gross.Dirty = true;

            _mgr.SavePayroll(payroll);
            _emprepos.DidNotReceive().Get();
            _dgrepos.Received(1).Insert(employee.Employee, gross);
            _dgrepos.DidNotReceive().Update(Arg.Any<Employee>(), Arg.Any<DailyGross>());
        }
    }
}
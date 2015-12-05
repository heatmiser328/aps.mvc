using System;
using System.Collections.Generic;
using System.Linq;

using ica.aps.core;
using ica.aps.data.models;

namespace Mocks
{
    internal static class MockData
    {
        private static Random random = new Random();
        internal static decimal RandomDecimal(decimal low, decimal high) 
        {
            return Math.Round((Convert.ToDecimal(random.NextDouble()) * (high - low)) + low, 2);
        }

        internal static IEnumerable<Employee> Employees
        {
            get
            {
                return new List<Employee>
                {
                    new Employee { EmployeeID = Guid.NewGuid(), Enabled = true, Name = "Frank", Modified=DateTime.Now, ModifiedBy="Admin", Title="Mr", Rents = MockData.Rents, Sequence=1},
                    new Employee { EmployeeID = Guid.NewGuid(), Enabled = true, Name = "Mary", Modified=DateTime.Now, ModifiedBy="Admin", Title="Ms", Rents = MockData.Rents, Sequence=2},
                    new Employee { EmployeeID = Guid.NewGuid(), Enabled = true, Name = "Jane", Modified=DateTime.Now, ModifiedBy="Admin", Title="Mrs", Rents = MockData.Rents, Sequence=3}
                };
            }
        }

        internal static IEnumerable<Rent> Rents
        {
            get
            {
                return new List<Rent> {
                    new Rent { RentID = Guid.NewGuid(), RentPct = RandomDecimal(0.5M, 0.9M), EffectiveTDS = DateTime.Now.AddMonths(-1), ModifiedBy = "admin", ModifiedTDS = DateTime.Now}
                };
            }
        }

        internal static DailyGross MockGross(DateTime dt)
        {
            return new DailyGross { DailyGrossID = Guid.NewGuid(), Gross = RandomDecimal(0, 500), GrossTDS = dt, Dirty = false, ModifiedTDS = DateTime.Now, ModifiedBy = "admin" };
        }

        internal static IEnumerable<DailyGross> Grosses(DateTime start, DateTime end)
        {
            var l = new List<DailyGross>();
            for (var dt = start; dt <= end; dt = dt.AddDays(1))
            {
                l.Add(MockData.MockGross(dt));
            }
            return l;
        }

        internal static Payroll Payroll
        {
            get
            {
                var ww = DateTime.Now.WorkWeek();

                var employees = new List<EmployeePayroll>(MockData.Employees.Select(e => {
                    return new EmployeePayroll(e, ww.Start)
                    {
                        Grosses = MockData.Grosses(ww.Start, ww.End)
                    };
                }));
                return new Payroll { Employees = employees, StartTDS = ww.Start, EndTDS = ww.End };
            }
        }
    }
}

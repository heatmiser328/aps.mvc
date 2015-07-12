using System;
using System.Data;
using System.Collections.Generic;

using ica.aps.core.interfaces;
using ica.aps.core.models;
using ica.aps.data.interfaces;

namespace ica.aps.data.repositories
{
    public class PayrollRepository : IPayrollRepository
    {
		private IEmployeeRepository _emprepos;
		private IDailyGrossRepository _dgrepos;
		
        public PayrollRepository(IEmployeeRepository emprepos, IDailyGrossRepository dgrepos)
        {
			_emprepos = emprepos;
			_dgrepos = dgrepos;
        }

		#region IPayrollRepository
        public IList<IEmployee> GetEmployees()
        {
			return _emprepos.GetEmployees();
        }

        public IPayroll GetPayroll(DateTime dt)
        {
            int offset = (int)dt.DayOfWeek - (int)System.DayOfWeek.Monday;
            int diff = (int)System.DayOfWeek.Saturday - (int)System.DayOfWeek.Monday;

			DateTime start = dt - new TimeSpan(offset, 0, 0, 0, 0);
            DateTime end = start.AddDays(diff);

			IPayroll payroll = new Payroll {
				StartTDS = start,
				EndTDS = end,
				Employees = new List<IEmployeePayroll>()
			};
			
            IList<IEmployee> employees = _emprepos.GetEmployees();
			foreach (IEmployee emp in employees)
			{
                IEmployeePayroll pr = new EmployeePayroll(emp, start);
                //pr.Rent = emp.EffectiveRent(start);
                pr.Grosses = _dgrepos.GetDailyGrosses(emp, start, end);
				
				payroll.Employees.Add(pr);
			}

			return payroll;
        }
		
        public void SavePayroll(IPayroll payroll)
        {
			foreach (IEmployeePayroll pr in payroll.Employees)
			{
                if (pr.Dirty)
                {
                    foreach (IDailyGross dg in pr.Grosses)
                    {
                        if (dg.Dirty)
                        {
                            if (dg.ID == null || dg.ID.Equals(Guid.Empty))
								_dgrepos.InsertDailyGross(pr.Employee, dg);
                            else
								_dgrepos.UpdateDailyGross(pr.Employee, dg);
                            dg.Dirty = false;
                        }
                    }
                }
			}
        }
		
        #endregion
    }
}

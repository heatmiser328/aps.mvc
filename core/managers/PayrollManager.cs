using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ica.aps.core.interfaces;
using ica.aps.data.interfaces;
using ica.aps.data.models;

namespace ica.aps.core.managers
{
    public class PayrollManager : IPayrollManager
    {
		private IEmployeeRepository _emprepos;
		private IDailyGrossRepository _dgrepos;
		
        public PayrollManager(IEmployeeRepository emprepos, IDailyGrossRepository dgrepos)
        {
			_emprepos = emprepos;
			_dgrepos = dgrepos;
        }

		#region IPayrollManager
        public IEnumerable<Employee> GetEmployees()
        {
			return _emprepos.GetEmployees();
        }

        public Payroll GetPayroll(DateTime dt)
        {
            int offset = (int)dt.DayOfWeek - (int)System.DayOfWeek.Monday;
            int diff = (int)System.DayOfWeek.Saturday - (int)System.DayOfWeek.Monday;

			DateTime start = dt - new TimeSpan(offset, 0, 0, 0, 0);
            DateTime end = start.AddDays(diff);

			Payroll payroll = new Payroll {
				StartTDS = start,
				EndTDS = end,
				Employees = new List<EmployeePayroll>()
			};
			
            var employees = _emprepos.GetEmployees();            
			foreach (Employee emp in employees)
			{
                EmployeePayroll pr = new EmployeePayroll(emp, start);                
                //pr.Rent = emp.EffectiveRent(start);                
                pr.Grosses = LoadDailyGrosses(emp, start, end);
				
				payroll.Employees.Add(pr);
			}

			return payroll;
        }
		
        public void SavePayroll(Payroll payroll)
        {
			foreach (EmployeePayroll pr in payroll.Employees)
			{
                if (pr.Dirty)
                {
                    foreach (DailyGross dg in pr.Grosses)
                    {
                        if (dg.Dirty)
                        {
                            if (dg.DailyGrossID == null || dg.DailyGrossID.Equals(Guid.Empty))
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

        private IEnumerable<DailyGross> LoadDailyGrosses(Employee emp, DateTime start, DateTime end)
        {
            var list = _dgrepos.GetDailyGrosses(emp, start, end);
            var grosses = new List<DailyGross>();
            for (var s = start; s <= end; s = s.AddDays(1)) 
            {
                var gross = list.Count() > 0
                    ? list.Where((g) => {return g.GrossDate.Year == s.Year && g.GrossDate.Month == s.Month && g.GrossDate.Day == s.Day;}).First()
                    : null;
                if (gross == null)
                {
                    gross = new DailyGross {
                        GrossDate = s,
                        GrossPay = 0,
                        Modified = DateTime.Now,
                        ModifiedBy = "admin",
                        Dirty = true
                    };
                }
                grosses.Add(gross);
            }

            return grosses;
        }

    }
}

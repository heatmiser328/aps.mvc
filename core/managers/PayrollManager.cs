using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ica.aps.core;
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
			return _emprepos.Get();
        }

        public Payroll GetPayroll(DateTime dt)
        {
            var ww = dt.WorkWeek();

			Payroll payroll = new Payroll {
				StartTDS = ww.Start,
				EndTDS = ww.End,
				Employees = new List<EmployeePayroll>()
			};
			
            var employees = _emprepos.Get();            
			foreach (Employee emp in employees)
			{
                EmployeePayroll pr = new EmployeePayroll(emp, ww.Start);                
                //pr.Rent = emp.EffectiveRent(start);                
                pr.Grosses = LoadDailyGrosses(emp, ww.Start, ww.End);
				
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
								_dgrepos.Insert(pr.Employee, dg);
                            else
								_dgrepos.Update(pr.Employee, dg);
                            dg.Dirty = false;
                        }
                    }
                }
			}
        }
		
        #endregion

        #region Implementation
        private IEnumerable<DailyGross> LoadDailyGrosses(Employee emp, DateTime start, DateTime end)
        {
            var list = _dgrepos.Get(emp, start, end);
            var grosses = new List<DailyGross>();
            for (var s = start; s <= end; s = s.AddDays(1)) 
            {
                var gross = list.Count() > 0
                    ? list.Where((g) => { return g.GrossTDS.Year == s.Year && g.GrossTDS.Month == s.Month && g.GrossTDS.Day == s.Day; }).FirstOrDefault()
                    : null;
                if (gross == null)
                {
                    gross = new DailyGross {
                        GrossTDS = s,
                        Gross = 0,
                        ModifiedTDS = DateTime.Now,
                        ModifiedBy = "admin",
                        Dirty = true
                    };
                }
                grosses.Add(gross);
            }

            return grosses;
        }
        #endregion
    }
}

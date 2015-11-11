using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ica.aps.data.models;

namespace ica.aps.core.interfaces
{
    public interface IPayrollManager
    {
        IEnumerable<Employee> GetEmployees();
        Payroll GetPayroll(DateTime dt);
        void SavePayroll(Payroll payroll);
    }
}

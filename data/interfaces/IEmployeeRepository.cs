using System;
using System.Collections.Generic;

using ica.aps.core.interfaces;

namespace ica.aps.data.interfaces
{
    public interface IEmployeeRepository
    {
        IList<IEmployee> GetEmployees();
    }
}

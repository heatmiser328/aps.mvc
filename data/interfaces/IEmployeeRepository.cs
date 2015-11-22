using System;
using System.Collections.Generic;

using ica.aps.data.models;

namespace ica.aps.data.interfaces
{
    public interface IEmployeeRepository
    {
        IEnumerable<Employee> Get();
    }
}

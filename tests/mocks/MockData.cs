using System;
using System.Collections.Generic;

using ica.aps.data.models;

namespace Mocks
{
    internal static class MockData
    {
        internal static IList<Employee> Employees
        {
            get
            {
                return new List<Employee>
                {
                    new Employee { EmployeeID = Guid.NewGuid(), Enabled = true, Name = "Frank", Modified=DateTime.Now, ModifiedBy="Admin", Title="Mr", Sequence=1},
                    new Employee { EmployeeID = Guid.NewGuid(), Enabled = true, Name = "Mary", Modified=DateTime.Now, ModifiedBy="Admin", Title="Ms", Sequence=2},
                    new Employee { EmployeeID = Guid.NewGuid(), Enabled = true, Name = "Jane", Modified=DateTime.Now, ModifiedBy="Admin", Title="Mrs", Sequence=3}
                };
            }
        }
    }
}

using System;
using System.Collections.Generic;

using ica.aps.core.interfaces;
using ica.aps.core.models;

namespace Mocks
{
    internal static class MockData
    {
        internal static IList<IEmployee> Employees
        {
            get
            {
                return new List<IEmployee>
                {
                    new Employee { ID = Guid.NewGuid(), Enabled = true, FirstName = "Frank", LastName= "Jones", Modified=DateTime.Now, ModifiedBy="Admin", Title="Mr", Sequence=1},
                    new Employee { ID = Guid.NewGuid(), Enabled = true, FirstName = "Mary", LastName= "Smith", Modified=DateTime.Now, ModifiedBy="Admin", Title="Ms", Sequence=2},
                    new Employee { ID = Guid.NewGuid(), Enabled = true, FirstName = "Jane", LastName= "Doe", Modified=DateTime.Now, ModifiedBy="Admin", Title="Mrs", Sequence=3}
                };
            }
        }
    }
}

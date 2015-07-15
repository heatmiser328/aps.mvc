using System;
using System.Data;
using System.Collections.Generic;

using ica.aps.core.interfaces;
using ica.aps.core.models;
using ica.aps.data.interfaces;
using ica.aps.orm;

namespace ica.aps.data.repositories
{
    public class EmployeeRepository : Repository, IEmployeeRepository
    {
		private IRentRepository _rrepo;
		
        public EmployeeRepository(IDatabase db, IRentRepository rrepo)
			: base(db)
        {
			_rrepo = rrepo;
        }
	
		#region IEmployeeRepository
        public IList<IEmployee> GetEmployees()
        {
            IList<IEmployee> employees = new List<IEmployee>();
			/*using (*/IDbConnection conn = this.Connection;//)
			{
	            using (IDbCommand cmd = conn.CreateCommand())
	            {
	                cmd.CommandText = cSelectEmployees_SQL;
	                using (IDataReader dr = cmd.ExecuteReader())
	                {
                        List<Employee> l = ORM.FillCollection<Employee, List<Employee>>(dr, (employee, row) => {                            
                            employee.Rents = _rrepo.GetRents(employee);
						});
                        employees = l.ConvertAll<IEmployee>(x => x);
	                }
	            }
			}

            return employees;
        }
        #endregion

        #region SQL

        private const string cSelectEmployees_SQL =
@"SELECT *
FROM Employee
ORDER BY Sequence";
        #endregion
    }
}

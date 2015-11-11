using System;
using System.Data;
using System.Collections.Generic;

using ica.aps.data.models;
using ica.aps.data.interfaces;
using Dapper;

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
        public IEnumerable<Employee> GetEmployees()
        {            
			/*using (*/IDbConnection conn = this.Connection;//)
			{
                var list = conn.Query<Employee>(cSelectEmployees_SQL);
                foreach (Employee e in list) 
                {                    
                    e.Rents = _rrepo.GetRents(e);                    
                }
                return list;
			}            
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

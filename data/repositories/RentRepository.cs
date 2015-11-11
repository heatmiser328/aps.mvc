using System;
using System.Data;
using System.Collections.Generic;

using ica.aps.data.models;
using ica.aps.data.interfaces;
using Dapper;

namespace ica.aps.data.repositories
{
    public class RentRepository : Repository, IRentRepository
    {
        public RentRepository(IDatabase db = null)
			: base(db)
        {
        }
	
        #region IRentRepository
        public IEnumerable<Rent> GetRents(Employee employee)
        {            
			/*using (*/IDbConnection conn = this.Connection;//)
			{
                return conn.Query<Rent>(cSelectRentsForEmployee_SQL, new { EmployeeID = employee.EmployeeID });
            }
        }
		#endregion

        #region SQL
        private const string cSelectRentsForEmployee_SQL =
@"SELECT *
FROM Rent
WHERE EmployeeID = @EmployeeID
ORDER BY EffectiveTDS DESC";
        #endregion
    }
}

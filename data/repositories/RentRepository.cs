using System;
using System.Data;
using System.Collections.Generic;

using ica.aps.core.interfaces;
using ica.aps.core.models;
using ica.aps.data.interfaces;
using ica.aps.orm;

namespace ica.aps.data.repositories
{
    public class RentRepository : Repository, IRentRepository
    {
        public RentRepository(IDatabase db = null)
			: base(db)
        {
        }
	
        #region IRentRepository
        public IList<IRent> GetRents(IEmployee employee)
        {
            IList<IRent> rents = new List<IRent>();
			/*using (*/IDbConnection conn = this.Connection;//)
			{
	            using (IDbCommand cmd = conn.CreateCommand())
	            {
	                cmd.CommandText = cSelectRentsForEmployee_SQL;
	                base.AddCommandParameter(cmd, "@EmployeeID", DbType.Guid, ParameterDirection.Input, employee.ID);
						
	                using (IDataReader dr = cmd.ExecuteReader())
	                {
                        List<Rent> l = ORM.FillCollection<Rent, List<Rent>>(dr);
                        rents = l.ConvertAll<IRent>(x => x);
                        //rents = ORM.FillCollection<Rent, List<Rent>>(dr);
	                }
	            }
			}

            return rents;
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

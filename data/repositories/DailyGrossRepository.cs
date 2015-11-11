using System;
using System.Data;
using System.Collections.Generic;

using ica.aps.data.models;
using ica.aps.data.interfaces;
using Dapper;

namespace ica.aps.data.repositories
{						  
    public class DailyGrossRepository : Repository, IDailyGrossRepository
    {
        public DailyGrossRepository(IDatabase db)
			: base(db)
        {
        }
	
		#region IDailyGrossRepository
        public IEnumerable<DailyGross> GetDailyGrosses(Employee employee, DateTime start, DateTime end)
        {
            var grosses = new List<DailyGross>();
			/*using (*/IDbConnection conn = this.Connection;//)
			{
                Rent r = employee.EffectiveRent(start);
                var list = conn.Query<DailyGross>(cSelectDailyGrossesForEmployee_SQL, new {
                    RentID = r.RentID, StartTDS = start, EndTDS = end
                });
			}

            return grosses;
        }

        public void InsertDailyGross(Employee employee, DailyGross dg)
        {
			/*using (*/IDbConnection conn = this.Connection;//)
			{
                /*
	            using (IDbCommand cmd = conn.CreateCommand())
	            {
					dg.ID = Guid.NewGuid();
					IRent r = employee.EffectiveRent(dg.GrossDate);
					
	                cmd.CommandText = cInsertDailyGrossForEmployee_SQL;
	                base.AddCommandParameter(cmd, "@DailyGrossID", DbType.Guid, ParameterDirection.Input, dg.ID);
	                base.AddCommandParameter(cmd, "@RentID", DbType.Guid, ParameterDirection.Input, r.ID);
	                base.AddCommandParameter(cmd, "@Gross", DbType.Decimal, ParameterDirection.Input, dg.GrossPay);
	                base.AddCommandParameter(cmd, "@GrossTDS", DbType.DateTime, ParameterDirection.Input, dg.GrossDate);
	                base.AddCommandParameter(cmd, "@ModifiedBy", DbType.String, ParameterDirection.Input, dg.ModifiedBy);
	                base.AddCommandParameter(cmd, "@ModifiedTDS", DbType.DateTime, ParameterDirection.Input, dg.Modified);
						
					cmd.ExecuteNonQuery();
	            }
                */ 
			}
        }
	
        public void UpdateDailyGross(Employee employee, DailyGross dg)
        {
			/*using (*/IDbConnection conn = this.Connection;//)
			{
                /*
	            using (IDbCommand cmd = conn.CreateCommand())
	            {
					IRent r = employee.EffectiveRent(dg.GrossDate);
					
	                cmd.CommandText = cUpdateDailyGrossForEmployee_SQL;
	                base.AddCommandParameter(cmd, "@DailyGrossID", DbType.Guid, ParameterDirection.Input, dg.ID);
	                base.AddCommandParameter(cmd, "@RentID", DbType.Guid, ParameterDirection.Input, r.ID);
	                base.AddCommandParameter(cmd, "@Gross", DbType.Decimal, ParameterDirection.Input, dg.GrossPay);
	                base.AddCommandParameter(cmd, "@GrossTDS", DbType.DateTime, ParameterDirection.Input, dg.GrossDate);
	                base.AddCommandParameter(cmd, "@ModifiedBy", DbType.String, ParameterDirection.Input, dg.ModifiedBy);
	                base.AddCommandParameter(cmd, "@ModifiedTDS", DbType.DateTime, ParameterDirection.Input, dg.Modified);
						
					cmd.ExecuteNonQuery();
	            }
                */ 
			}
        }
        #endregion
		
        #region SQL

        private const string cSelectDailyGrossesForEmployee_SQL =
@"SELECT *
FROM DailyGross
WHERE RentID = @RentID AND GrossTDS between @StartTDS and @EndTDS
ORDER BY GrossTDS";

        private const string cInsertDailyGrossForEmployee_SQL = 
@"INSERT INTO [DailyGross] 
	([DailyGrossID], [RentID], [Gross], [GrossTDS], [ModifiedBy], [ModifiedTDS])
VALUES
	(@DailyGrossID, @RentID, @Gross, @GrossTDS, @ModifiedBy, @ModifiedTDS)";

        private const string cUpdateDailyGrossForEmployee_SQL = 
@"UPDATE [DailyGross] SET 
	[RentID] = @RentID, 
	[Gross] = @Gross, 
	[GrossTDS] = @GrossTDS, 
	[ModifiedBy] = @ModifiedBy, 
	[ModifiedTDS] = @ModifiedTDS
WHERE
	[DailyGrossID] = @DailyGrossID";
	
        #endregion
    }
}

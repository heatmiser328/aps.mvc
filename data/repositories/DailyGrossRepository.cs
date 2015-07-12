using System;
using System.Data;
using System.Collections.Generic;

using ica.aps.core.interfaces;
using ica.aps.core.models;
using ica.aps.data.interfaces;
using ica.aps.orm;

namespace ica.aps.data.repositories
{						  
    public class DailyGrossRepository : Repository, IDailyGrossRepository
    {
        public DailyGrossRepository(IDatabase db)
			: base(db)
        {
        }
	
		#region IDailyGrossRepository
        public IList<IDailyGross> GetDailyGrosses(IEmployee employee, DateTime start, DateTime end)
        {
            IList<DailyGross> grosses = new List<DailyGross>();
			/*using (*/IDbConnection conn = this.Connection;//)
			{
	            using (IDbCommand cmd = conn.CreateCommand())
	            {
					IRent r = employee.EffectiveRent(start);
					
	                cmd.CommandText = cSelectDailyGrossesForEmployee_SQL;
	                base.AddCommandParameter(cmd, "@RentID", DbType.Guid, ParameterDirection.Input, r.ID);
	                base.AddCommandParameter(cmd, "@StartTDS", DbType.DateTime, ParameterDirection.Input, start);
	                base.AddCommandParameter(cmd, "@EndTDS", DbType.DateTime, ParameterDirection.Input, end);
						
	                using (IDataReader dr = cmd.ExecuteReader())
	                {
						grosses = ORM.FillCollection<DailyGross, IList<DailyGross>>(dr);
	                }
	            }
			}

            return grosses;
        }

        public void InsertDailyGross(IEmployee employee, IDailyGross dg)
        {
			/*using (*/IDbConnection conn = this.Connection;//)
			{
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
			}
        }
	
        public void UpdateDailyGross(IEmployee employee, IDailyGross dg)
        {
			/*using (*/IDbConnection conn = this.Connection;//)
			{
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

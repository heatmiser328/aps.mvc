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
        public IEnumerable<DailyGross> Get(Employee employee, DateTime start, DateTime end)
        {            
			/*using (*/IDbConnection conn = this.Connection;//)
			{
                Rent r = employee.EffectiveRent(start);
                return conn.Query<DailyGross>(cSelectDailyGrossesForEmployee_SQL, new {
                    RentID = r.RentID, StartTDS = start, EndTDS = end
                });
			}
         
        }

        public void Insert(Employee employee, DailyGross dg)
        {
			/*using (*/IDbConnection conn = this.Connection;//)
			{
                dg.DailyGrossID = Guid.NewGuid();                
                conn.Execute(cInsertDailyGrossForEmployee_SQL, new {
	                DailyGrossID=dg.DailyGrossID,
	                RentID=employee.EffectiveRent(dg.GrossTDS).RentID,
	                Gross=dg.Gross,
	                GrossTDS=dg.GrossTDS,
	                ModifiedBy=dg.ModifiedBy,
	                ModifiedTDS=dg.ModifiedTDS
                });
			}
        }
	
        public void Update(Employee employee, DailyGross dg)
        {
			/*using (*/IDbConnection conn = this.Connection;//)
			{
                conn.Execute(cUpdateDailyGrossForEmployee_SQL, new {
                    DailyGrossID = dg.DailyGrossID,
                    RentID = employee.EffectiveRent(dg.GrossTDS).RentID,
                    Gross = dg.Gross,
                    GrossTDS = dg.GrossTDS,
                    ModifiedBy = dg.ModifiedBy,
                    ModifiedTDS = dg.ModifiedTDS
                });
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

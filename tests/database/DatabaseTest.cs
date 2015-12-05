using System;
using System.Data;

using Xunit;
using Shouldly;

using ica.aps.data.interfaces;
using ica.aps.data.db;

namespace Data
{
    [Trait("category", "Database")]
    public class DataTest
    {
        private const string _provider = "System.Data.SqlClient";
        private const string _connstring = "Data Source=dev-s01;Initial Catalog=master;User ID=sa;Password=sql@dm1n";
		
        [Fact]        
        public void Instantiate()
        {
			IDatabase db = new Database(_provider, _connstring);
			db.ShouldNotBe(null);
			db.IsSqlServerProvider.ShouldBe(true);
			db.IsSqlServerCeProvider.ShouldBe(false);
			db.IsOracleProvider.ShouldBe(false);
        }
		
        [Fact]
        public void Create()
        {
			IDatabase db = new Database(_provider, _connstring);
            IDbConnection conn = db.Create();
			conn.ShouldNotBe(null);
            conn.State.ShouldBe(ConnectionState.Closed);
        }
		
        [Fact]
        public void UseConnection()
        {
			IDatabase db = new Database(_provider, _connstring);
			using (IDbConnection conn = db.Create())
			{
				conn.ShouldNotBe(null);
                conn.Open();
                conn.State.ShouldBe(ConnectionState.Open);
				
	            using (IDbCommand cmd = conn.CreateCommand())
	            {
	                cmd.CommandText = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES";
					var count = cmd.ExecuteScalar();
                    count.ShouldBeAssignableTo<int>();
                    count.ShouldNotBe(0);
	            }
			}
        }
		
    }
}

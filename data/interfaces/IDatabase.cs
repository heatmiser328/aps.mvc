using System;
using System.Data;

namespace ica.aps.data.interfaces
{
    /// <summary>
    /// IDatabase
    /// </summary>
    public interface IDatabase
    {
		bool IsSqlServerProvider {get;}
        bool IsSqlServerCeProvider {get;}
        bool IsOracleProvider {get;}
        IDbConnection Create();
    }
}

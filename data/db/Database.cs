using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;

using ica.aps.data.interfaces;

namespace ica.aps.data.db
{
    /// <summary>
    /// Database
    /// </summary>
    public class Database : IDatabase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="connectionString"></param>
        public Database(string provider, string connectionString)
        {
            _provider = provider;
            _connectionString = connectionString;
            _factory = DbProviderFactories.GetFactory(provider);
        }

        #region IDatabase
        /// <summary>
        /// Is Sql Server Provider
        /// </summary>
        public bool IsSqlServerProvider
        {
            get { return IsProvider(cSQLProviderName); }
        }
        /// <summary>
        /// Is Sql Server CE Provider
        /// </summary>
        public bool IsSqlServerCeProvider
        {
            get { return IsProvider(cSQLCEProviderName); }
        }
        /// <summary>
        /// Is Oracle Provider
        /// </summary>
        public bool IsOracleProvider
        {
            get { return IsProvider(cOracleProviderName); }
        }

        /// <summary>
        /// Retrieves a database connection
        /// </summary>
        /// <returns>IDbConnection reference to a specific provider connection object</returns>
        public IDbConnection Create()
        {
            var conn = _factory.CreateConnection();
            conn.ConnectionString = _connectionString;
            return conn;
        }
        #endregion

        #region Implementation
        bool IsProvider(string providerName)
        {
            return IsProvider(_provider, providerName);
        }
        bool IsProvider(string provider, string providerName)
        {
            return (string.Compare(provider, providerName, true) == 0);
        }
        #endregion

        #region Private Properties
        const string cSQLProviderName = "System.Data.SqlClient";
        const string cSQLCEProviderName = "System.Data.SqlServerCe";
        const string cOracleProviderName = "System.Data.OracleClient";

        string _provider;
        string _connectionString;
        DbProviderFactory _factory;
        #endregion
    }
}

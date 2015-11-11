using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;

using ica.aps.data.interfaces;

namespace ica.aps.data.repositories
{
	public abstract class Repository
	{
		private IDatabase _db;
		private IDbConnection _conn;

        protected Repository(IDatabase db)
		{
			_db = db;
		}
		
		protected IDbConnection Connection
		{
			get
			{
				if (_conn == null)
				{
                    _conn = _db.Create();
					_conn.Open();
				}
				return _conn;
			}
		}
		
		protected IDbConnection Open()
		{
			return this.Connection;
		}
		
		protected void Close()
		{
			if (_conn != null)
			{
				_conn.Close();
				_conn = null;
			}
		}
		
        /// <summary>
        /// Add a Parameter to a Command
        /// </summary>
        /// <param name="cmd">SQL command </param>
        /// <param name="name">Name of the @parameter in the SQL command</param>
        /// <param name="type">Data type of the parameter</param>
        /// <param name="direction">Parameter direction</param>
        /// <param name="value">Value to assign to the parameter</param>
        /// <returns>Returns the parameter object</returns>
        protected IDbDataParameter AddCommandParameter(IDbCommand command, string name, DbType dataType, ParameterDirection direction, Object value)
        {
            IDbDataParameter param = command.CreateParameter();
            param.DbType = dataType;
            param.ParameterName = name;
            param.Value = value;
            param.Direction = direction;
            command.Parameters.Add(param);

            return param;
        }
	}
}


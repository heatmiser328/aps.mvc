using System;
using System.Data;
using System.Collections.Generic;

using ica.aps.core.interfaces;
using ica.aps.core.models;
using ica.aps.data.interfaces;
using ica.aps.orm;

namespace ica.aps.data.repositories
{
    public class UserRepository : Repository, IUserRepository
    {		
        public UserRepository(IDatabase db)
			: base(db)
        {	
        }
	
		#region IUserRepository
        public IUser GetUser(string username)
        {
            IUser user = null;
			/*using (*/IDbConnection conn = this.Connection;//)
			{
	            using (IDbCommand cmd = conn.CreateCommand())
	            {
	                cmd.CommandText = string.Format(cSelectUser_SQL, username);
	                using (IDataReader dr = cmd.ExecuteReader())
	                {                        
                        user = ORM.FillObject<User>(dr);
	                }
	            }
			}

            return user;
        }
        #endregion

        #region SQL

        private const string cSelectUser_SQL =
@"SELECT *
FROM User
WHERE UserName = '{0}'";
        #endregion
    }
}

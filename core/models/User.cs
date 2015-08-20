using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;

using ica.aps.core.interfaces;

namespace ica.aps.core.models
{
    [DataContract]
    public class User : IUser
    {
        [DataMember]
        public string FirstName
        {
            get;
            set;
        }
        [DataMember]
        public string LastName
        {
            get;
            set;
        }
        [DataMember]
        public string UserName
        {
            get;
            set;
        }
        [DataMember]
        public string salt
        {
            get;
            set;
        }
        [DataMember]
        public string hash
        {
            get;
            set;
        }

        public static IUser Create(string firstname, string lastname, string username, string password)
        {
            IUser user = new User { FirstName = firstname, LastName = lastname, UserName = username };

            // Generate a random salt
            user.salt = Guid.NewGuid().ToString();
            user.hash = HashPassword(password, user.salt);

            return user;
        }

        public static string HashPassword(string password, string salt)
        {
            byte[] _salt = Encoding.UTF8.GetBytes(salt);            
            // Generate the hash
            Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, _salt);
            rfc2898DeriveBytes.IterationCount = 10000;
            byte[] hash = rfc2898DeriveBytes.GetBytes(20);
            //Return the hash
            return Convert.ToBase64String(hash);
        }        
    }
}

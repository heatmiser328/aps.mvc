using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ica.aps.core.interfaces
{
    public interface IUser
    {
        string FirstName { get; set; }
        string LastName { get; set; }
        string UserName { get; set; }
        string salt { get; set; }
        string hash { get; set; }
    }
}

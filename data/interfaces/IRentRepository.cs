using System;
using System.Collections.Generic;

using ica.aps.data.models;

namespace ica.aps.data.interfaces
{						  
    public interface IRentRepository
    {
        IEnumerable<Rent> Get(Employee employee);
    }
}

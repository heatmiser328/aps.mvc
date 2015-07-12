using System;
using System.Collections.Generic;

using ica.aps.core.interfaces;

namespace ica.aps.data.interfaces
{						  
    public interface IRentRepository
    {
        IList<IRent> GetRents(IEmployee employee);
    }
}

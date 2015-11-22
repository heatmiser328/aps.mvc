using System;
using System.Collections.Generic;

using ica.aps.data.models;

namespace ica.aps.data.interfaces
{						  
    public interface IDailyGrossRepository
    {
        IEnumerable<DailyGross> Get(Employee employee, DateTime start, DateTime end);
        void Insert(Employee employee, DailyGross dg);
        void Update(Employee employee, DailyGross dg);
    }
}

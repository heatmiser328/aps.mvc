using System;
using System.Collections.Generic;

using ica.aps.data.models;

namespace ica.aps.data.interfaces
{						  
    public interface IDailyGrossRepository
    {
        IEnumerable<DailyGross> GetDailyGrosses(Employee employee, DateTime start, DateTime end);
        void InsertDailyGross(Employee employee, DailyGross dg);
        void UpdateDailyGross(Employee employee, DailyGross dg);
    }
}

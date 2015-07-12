using System;
using System.Collections.Generic;

namespace ica.aps.core.interfaces
{
    public interface IEmployeePayroll
    {
		IEmployee Employee {get;set;}        
		IList<IDailyGross> Grosses {get;set;}

        decimal Gross { get; }
        decimal Net { get; }
        decimal Rent { get; }

        bool Dirty { get; }
    }
}

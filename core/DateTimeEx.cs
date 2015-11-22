using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ica.aps.core
{
    public class WorkWeek
    {
        public WorkWeek(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }
        public DateTime Start {get; private set;}
        public DateTime End { get; private set; }
    }

    public static class DateTimeEx
    {
        public static WorkWeek WorkWeek(this DateTime dt)
        {
            DateTime d = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0);
            int offset = (int)dt.DayOfWeek - (int)System.DayOfWeek.Monday;
            int diff = (int)System.DayOfWeek.Saturday - (int)System.DayOfWeek.Monday;
            
            DateTime start = d - new TimeSpan(offset, 0, 0, 0, 0);
            DateTime end = start.AddDays(diff).AddHours(23).AddMinutes(59).AddSeconds(59);

            return new WorkWeek(start, end);
        }
    }
}

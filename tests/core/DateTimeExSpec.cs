using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NSpec;
using Shouldly;

using ica.aps.core;

namespace Core
{    
    class DateTimeEx_Spec : nspec
    {
        DateTime _now;
        void before_each()
        {
            _now = DateTime.Now;
        }

        void work_week()
        {
            WorkWeek ww = null;
            beforeEach = () => ww = _now.WorkWeek();

            it["should not be null"] = () => {
                ww.ShouldNotBe(null);
            };

            it["should start on monday"] = () => {
                ww.Start.DayOfWeek.ShouldBe(System.DayOfWeek.Monday);
            };

            it["should end on saturday"] = () => {
                ww.End.DayOfWeek.ShouldBe(System.DayOfWeek.Saturday);
            };                       
        }
    }
}

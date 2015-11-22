using System;
using System.Collections.Generic;

using Xunit;
using Shouldly;

using ica.aps.core;

namespace Core
{ 
    public class DateTimeExTest
    {
        public DateTimeExTest()
        {
        }
        
        [Fact]
        public void WorkWeek()
        {
            DateTime now = DateTime.Now;

            var ww = now.WorkWeek();
            ww.ShouldNotBe(null);
            ww.Start.DayOfWeek.ShouldBe(System.DayOfWeek.Monday);
            ww.End.DayOfWeek.ShouldBe(System.DayOfWeek.Saturday);
        }
    }
}
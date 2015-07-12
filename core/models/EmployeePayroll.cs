using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using ica.aps.core.interfaces;

namespace ica.aps.core.models
{
	[DataContract]
    public class EmployeePayroll : IEmployeePayroll
    {
        private DateTime _start;
	
        public EmployeePayroll(IEmployee emp, DateTime start) 
        {
            this.Employee = emp;
            _start = start;
        }

		[DataMember]
        public IEmployee Employee { get; set; }
		[DataMember]
        public IList<IDailyGross> Grosses { get; set; }

		[DataMember]
        public decimal Gross
        {
            get 
            {
                return this.Grosses.Sum(g => g.GrossPay);                
            }
        }

		[DataMember]
        public decimal Net
        {
            get
            {
                return this.Gross * (1.0M - this.Employee.EffectiveRent(_start).RentPct);
            }
        }

		[DataMember]
        public decimal Rent
        {
            get
            {
                return this.Gross * this.Employee.EffectiveRent(_start).RentPct;
            }
        }

		[DataMember]
        public bool Dirty
        {
            get
            {
                return this.Grosses.Any(g => g.Dirty);
            }
        }
    }
}

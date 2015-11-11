using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ica.aps.data.models
{
    [DataContract]
    public class EmployeePayroll
    {
        private DateTime _start;
	
        public EmployeePayroll(Employee emp, DateTime start) 
        {
            this.Employee = emp;
            _start = start;
        }

        [DataMember]
        public Employee Employee { get; set; }
        [DataMember]
        public IEnumerable<DailyGross> Grosses { get; set; }

        [DataMember]
        public decimal RentRate
        {
            get
            {
                return this.Employee.EffectiveRent(_start).RentPct;
            }
        }


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
                return this.Gross * this.RentRate;
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

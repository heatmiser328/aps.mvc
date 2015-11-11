using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ica.aps.data.models
{
    [DataContract]
    public class Payroll
    {
        [DataMember]
        public DateTime StartTDS {get;set;}
        [DataMember]
        public DateTime EndTDS {get;set;}
        [DataMember]
        public IList<EmployeePayroll> Employees {get;set;}

        [DataMember]
        public decimal Gross
        {
            get 
            {
                return this.Employees.Sum(e => e.Gross);
            }
        }

        [DataMember]
        public decimal Net
        {
            get
            {
                return this.Employees.Sum(e => e.Net);
            }
        }

        [DataMember]
        public decimal Rent
        {
            get
            {
                return this.Employees.Sum(e => e.Rent);
            }
        }
    }
}

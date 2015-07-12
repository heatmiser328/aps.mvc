using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using ica.aps.core.interfaces;

namespace ica.aps.core.models
{
	[DataContract]
    public class Payroll : IPayroll
    {
		[DataMember]
        public DateTime StartTDS {get;set;}
		[DataMember]
        public DateTime EndTDS {get;set;}
		[DataMember]
        public IList<IEmployeePayroll> Employees {get;set;}

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

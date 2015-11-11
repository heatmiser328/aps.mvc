using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ica.aps.data.models
{
    [DataContract]
    public class Employee
    {
        public Guid? EmployeeID { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string Title { get; set; }
        public int Sequence { get; set; }
        [DataMember]
        public IEnumerable<Rent> Rents { get; set; }
        [DataMember]
        public bool Enabled { get; set; }
        [DataMember]
        public DateTime Modified { get; set; }
        [DataMember]
        public string ModifiedBy { get; set; }

        [DataMember]
        public string FullName
        {
            get 
            {
                return this.FirstName + " " + this.LastName;
            }
        }

        public Rent EffectiveRent(DateTime? dt = null)
        {
            if (this.Rents == null || this.Rents.Count() < 1) 
                return null;

            if (dt == null || !dt.HasValue)            
                dt = DateTime.Now;
            
            var rent =
                from r in this.Rents
                where r.EffectiveTDS <= dt
                orderby r.EffectiveTDS descending
                select r;

            if (rent != null) 
                return rent.First();
            return this.Rents.First();
        }
    }
}

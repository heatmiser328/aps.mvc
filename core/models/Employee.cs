using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

using ica.aps.core.interfaces;

namespace ica.aps.core.models
{
	[DataContract]
    public class Employee : IEmployee
    {
		[DataMember]
        public Guid? ID { get; set; }
		[DataMember]
        public string FirstName { get; set; }
		[DataMember]
        public string LastName { get; set; }
		[DataMember]
        public string Title { get; set; }
		[DataMember]
        public int Sequence { get; set; }
		[DataMember]
        public IList<IRent> Rents { get; set; }
		[DataMember]
        public bool Enabled { get; set; }
		[DataMember]
        public DateTime Modified { get; set; }
		[DataMember]
        public string ModifiedBy { get; set; }

        public string FullName
        {
            get 
            {
                return this.FirstName + " " + this.LastName;
            }
        }

        public IRent EffectiveRent(DateTime? dt = null)
        {
            if (this.Rents == null || this.Rents.Count < 1) 
                return null;

            if (dt == null || !dt.HasValue)            
                dt = DateTime.Now;
            
            var rent =
                from r in this.Rents
                where r.EffectiveDate <= dt
                orderby r.EffectiveDate descending
                select r;

            if (rent != null) 
                return rent.First();
            return this.Rents[0];
        }
    }
}

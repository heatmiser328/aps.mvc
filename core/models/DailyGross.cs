using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using ica.aps.core.interfaces;

namespace ica.aps.core.models
{
	[DataContract]
    public class DailyGross : IDailyGross
    {
		[DataMember]
        public Guid? ID { get; set; }        
		[DataMember]
        public DateTime GrossDate { get; set; }
		[DataMember]
        public decimal GrossPay { get; set; }
		[DataMember]
        public DateTime Modified { get; set; }
		[DataMember]
        public string ModifiedBy { get; set; }
		[DataMember]
        public bool Dirty { get; set; }
    }
}

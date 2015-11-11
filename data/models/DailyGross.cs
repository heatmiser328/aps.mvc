using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ica.aps.data.models
{
    [DataContract]
    public class DailyGross
    {
        public Guid? DailyGrossID { get; set; }        
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

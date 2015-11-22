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
        public DateTime GrossTDS { get; set; }
        [DataMember]
        public decimal Gross { get; set; }
        [DataMember]
        public DateTime Modified { get; set; }
        [DataMember]
        public string ModifiedBy { get; set; }
        [DataMember]
        public bool Dirty { get; set; }
    }
}

using System;
using System.Runtime.Serialization;

namespace ica.aps.data.models
{
    [DataContract]
    public class Rent
    {
        public Guid? RentID { get; set; }
        [DataMember]
        public decimal RentPct { get; set; }
        [DataMember]
        public DateTime EffectiveTDS { get; set; }
        [DataMember]
        public DateTime ModifiedTDS { get; set; }
        [DataMember]
        public string ModifiedBy { get; set; }
    }
}

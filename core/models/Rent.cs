using System;
using System.Runtime.Serialization;

using ica.aps.core.interfaces;
using ica.aps.orm.attributes;

namespace ica.aps.core.models
{
	[DataContract]
    public class Rent : IRent
    {
		[DataMember]
		[DataMapping("RentID")]
        public Guid? ID { get; set; }
		[DataMember]
        public decimal RentPct { get; set; }
		[DataMember]
		[DataMapping("EffectiveTDS")]
        public DateTime EffectiveDate { get; set; }
		[DataMember]
		[DataMapping("ModifiedTDS")]
        public DateTime Modified { get; set; }
		[DataMember]
        public string ModifiedBy { get; set; }
    }
}

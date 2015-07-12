using System;

namespace ica.aps.core.interfaces
{
    public interface IRent
    {
        Guid? ID { get; set; }
        decimal RentPct { get; set; }
        DateTime EffectiveDate { get; set; }
        DateTime Modified { get; set; }
        string ModifiedBy { get; set; }
    }
}

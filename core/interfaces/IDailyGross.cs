using System;

namespace ica.aps.core.interfaces
{
    public interface IDailyGross
    {
        Guid? ID { get; set; }        
        DateTime GrossDate { get; set; }
        decimal GrossPay { get; set; }        
        DateTime Modified { get; set; }
        string ModifiedBy { get; set; }

        bool Dirty { get; set; }
    }
}

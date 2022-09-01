using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Domain
{
    // Service table for Preferences - PromoCodes Many-To-Many relation
    public class CustomerPreference
    {
        public Guid CustomerId { get; set; }
        public Guid PreferenceId { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Preference Preference { get; set; }
    }
}

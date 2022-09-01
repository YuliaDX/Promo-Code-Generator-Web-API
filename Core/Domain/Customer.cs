using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Domain
{
    public class Customer
         : BaseEntity
    {
        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        [MaxLength(50)]
        public string Email { get; set; }

        // Customer - PromoCodes One-To-Many relation
        public virtual List<PromoCode> PromoCodes { get; set; }

        //Preferences - PromoCodes Many-To-Many relation
        // CustomerPreference - table with keys
        public virtual List<CustomerPreference> Preferences { get; set; }
    }
}

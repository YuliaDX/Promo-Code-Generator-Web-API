using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core
{
    public class Employee
        : BaseEntity
    {
        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";
        
        [MaxLength(50)]
        public string Email { get; set; }

        public virtual List<Role> Roles { get; set; }

        public int AppliedPromocodesCount { get; set; }
    }
}

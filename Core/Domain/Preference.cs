using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Domain
{
    public class Preference
       : BaseEntity
    {
        [MaxLength(100)]
        public string Name { get; set; }
    }
}

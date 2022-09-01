using System;
using System.ComponentModel.DataAnnotations;

namespace Core
{
    public class Role
        : BaseEntity
    {
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }
    }
}

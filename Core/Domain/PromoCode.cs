using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Domain
{
    public class PromoCode
        : BaseEntity
    {
        [MaxLength(20)]
        public string Code { get; set; }

        [MaxLength(200)]
        public string ServiceInfo { get; set; }

        public DateTime BeginDate { get; set; }

        public DateTime EndDate { get; set; }

        [MaxLength(50)]
        public string PartnerName { get; set; }

        public virtual Employee PartnerManager { get; set; }

        public virtual Preference Preference { get; set; }
        public Guid CustomerId { get; set; }
    }
}

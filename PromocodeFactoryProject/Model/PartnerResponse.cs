using System;
using System.Collections.Generic;

namespace PromocodeFactoryProject.Controllers
{
    public class PartnerResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public int NumberIssuedPromoCodes { get; set; }

        public bool IsActive { get; set; }

        public List<PartnerPromoCodeLimitResponse> PartnerLimits { get; set; }
    }
}

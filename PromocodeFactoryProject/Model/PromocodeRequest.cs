using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromocodeFactoryProject.Model
{
    public class PromoCodeRequest
    {
        public string ServiceInfo { get; set; }

        public string PartnerName { get; set; }

        public string PromoCode { get; set; }

        public string Preference { get; set; }

        //id of admin or partner manager
        public Guid PartnerManagerId { get; set; }
    }

}

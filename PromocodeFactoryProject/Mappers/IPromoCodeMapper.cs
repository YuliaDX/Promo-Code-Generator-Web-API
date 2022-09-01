using Core.Domain;
using PromocodeFactoryProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromocodeFactoryProject.Mappers
{
    public interface IPromoCodeMapper
    {
        public PromoCode MapFromModel(PromoCodeRequest promoCodeRequest, IEnumerable<Preference> preferences, 
            PromoCode promoCode = null);
    }
    public class PromoCodeMapper : IPromoCodeMapper
    {
        public PromoCode MapFromModel(PromoCodeRequest promoCodeRequest, IEnumerable<Preference> preferences,
            PromoCode promoCode = null)
        {
            if(promoCode == null)
            {
                promoCode = new PromoCode();
                promoCode.Id = Guid.NewGuid();
            }

            promoCode.Code = promoCodeRequest.PromoCode;
            promoCode.PartnerName = promoCodeRequest.PartnerName;
            promoCode.ServiceInfo = promoCodeRequest.ServiceInfo;
            promoCode.Preference = preferences.FirstOrDefault(p => p.Name == promoCodeRequest.Preference);
            return promoCode;
        }
    }
}

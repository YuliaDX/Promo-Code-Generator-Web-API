using Core;
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
        PromoCode MapPromoCodeFromModel(PromoCodeRequest promoCodeRequest, Preference preference,
            Employee employee, PromoCode promoCode = null);
        PromoCodeResponse MapPromocodeToDTO(PromoCode promoCode);
    }
    public class PromoCodeMapper : IPromoCodeMapper
    {
        public PromoCode MapPromoCodeFromModel(PromoCodeRequest promoCodeRequest, Preference preference,
            Employee employee, PromoCode promoCode = null)
        {
            if(promoCode == null)
            {
                promoCode = new PromoCode();
                promoCode.Id = Guid.NewGuid();
            }

            promoCode.Code = promoCodeRequest.PromoCode;
            promoCode.PartnerName = promoCodeRequest.PartnerName;
            promoCode.ServiceInfo = promoCodeRequest.ServiceInfo;
            promoCode.Preference = preference;
            promoCode.PartnerManager = employee;
            promoCode.BeginDate = DateTime.Now;
            promoCode.EndDate = promoCode.BeginDate.AddMonths(2);
            return promoCode;
        }

        public PromoCodeResponse MapPromocodeToDTO(PromoCode promoCode) =>
             new PromoCodeResponse()
             {
                 Id = promoCode.Id,
                 Code = promoCode.Code,
                 ServiceInfo = promoCode.ServiceInfo,
                 BeginDate = promoCode.BeginDate.ToString("yyyy-MM-dd"),
                 EndDate = promoCode.EndDate.ToString("yyyy-MM-dd"),
                 PartnerName = promoCode.PartnerName

             };
    }
}

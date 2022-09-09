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
        public PromoCode MapFromModel(PromoCodeRequest promoCodeRequest, IEnumerable<Preference> preferences, 
            IEnumerable<Employee> employees, PromoCode promoCode = null);
    }
    public class PromoCodeMapper : IPromoCodeMapper
    {
        public PromoCode MapFromModel(PromoCodeRequest promoCodeRequest, IEnumerable<Preference> preferences,
            IEnumerable<Employee> employees, PromoCode promoCode = null)
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
            promoCode.PartnerManager = employees.FirstOrDefault(e=> e.Id == promoCodeRequest.PartnerManagerId);
            promoCode.BeginDate = DateTime.Now;
            promoCode.EndDate = promoCode.BeginDate.AddMonths(2);
            return promoCode;
        }
    }
}

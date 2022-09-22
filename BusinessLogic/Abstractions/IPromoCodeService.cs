using Core;
using Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Abstractions
{
    public interface IPromoCodeService
    {
        Task<IEnumerable<PromoCode>> GetAllPromoCodesAsync();
        Task CreatePromoCodeAsync(PromoCode promoCode);
        Task<Preference> GetPreferenceAsync(string preference);
        Task<Employee> GetPartnerManagerAsync(Guid partnerManagerId);
        Task GivePromoCodesToCustomersAsync(PromoCode promoCode);
    }
}

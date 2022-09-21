using Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Abstractions
{
    public interface IPartnerService
    {
        Task<IEnumerable<Partner>> GetAllPartnersAsync();
        Task<Partner> GetPartnerAsync(Guid id);
        PartnerPromoCodeLimit GetPartnerPromoCodeLimit(Partner partner, Guid limitId);
        Task<PartnerPromoCodeLimit> SetPartnerPromoCodeLimitAsync(int limit, DateTime endDate, Partner partner);
        Task CancelPartnerPromoCodeLimitAsync(Partner partner);
    }


}

using Core.Domain;
using PromocodeFactoryProject.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromocodeFactoryProject.Mappers
{
    public interface IPartnerMapper
    {
        PartnerResponse MapPartnerEntityToDTO(Partner partner);
        PartnerPromoCodeLimitResponse MapPromoCodeLimitToDTO(PartnerPromoCodeLimit partnerLimit);
    }

    public class PartnerMapper : IPartnerMapper
    {
        public PartnerResponse MapPartnerEntityToDTO(Partner x)
        {
            PartnerResponse model = new PartnerResponse()
            {
                Id = x.Id,
                Name = x.Name,
                NumberIssuedPromoCodes = x.NumberIssuedPromoCodes,
                IsActive = x.IsActive,
                PartnerLimits = x.PartnerLimits.Select(partnerLimit => MapPromoCodeLimitToDTO(partnerLimit)).ToList()

            };
            return model;
        }
        public PartnerPromoCodeLimitResponse MapPromoCodeLimitToDTO(PartnerPromoCodeLimit partnerLimit)
        {
            PartnerPromoCodeLimitResponse limitResponse = new PartnerPromoCodeLimitResponse()
            {
                Id = partnerLimit.Id,
                PartnerId = partnerLimit.PartnerId,
                CreateDate = partnerLimit.CreateDate.ToString("dd.MM.yyyy hh:mm:ss"),
                CancelDate = partnerLimit.CancelDate?.ToString("dd.MM.yyyy hh:mm:ss"),
                EndDate = partnerLimit.EndDate.ToString("dd.MM.yyyy hh:mm:ss"),
                Limit = partnerLimit.Limit
            };
            return limitResponse;
        }
    }
}

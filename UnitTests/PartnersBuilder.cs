using Core.Domain;
using PromocodeFactoryProject.Controllers;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTests
{
    public static class PartnersBuilder
    {
        public static Partner CreateBasePartner(Guid id, bool isActive)
        {
            Partner partner = new Partner()
            {
                Id = id,
                Name = "Toys Cash and Carry",
                IsActive = isActive,
                PartnerLimits = new List<PartnerPromoCodeLimit>() {
                      new PartnerPromoCodeLimit()
                    {
                        Id = Guid.Parse("ef7f299f-d8d5-459f-896e-eb9f14e1a32f"),
                        CreateDate = new DateTime(2022, 2, 25),
                        EndDate = new DateTime(2022,5,25),
                        Limit = 100
                    }
                }
            };
            return partner;
        }
        public static PartnerPromoCodeLimitRequest CreatePartnerLimit(int limit, DateTime endDate)
        {
            PartnerPromoCodeLimitRequest partnerPromoCodeLimitRequest = new PartnerPromoCodeLimitRequest();
            partnerPromoCodeLimitRequest.Limit = limit;
            partnerPromoCodeLimitRequest.EndDate = endDate;
            return partnerPromoCodeLimitRequest;
        }
    }
}

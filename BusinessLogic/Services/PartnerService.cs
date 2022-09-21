using BusinessLogic.Abstractions;
using Core.Domain;
using Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class PartnerService : IPartnerService
    {
        readonly IRepository<Partner> _partnerRepository;
        readonly ICurrentDateTimeProvider _currentDateTimeProvider;
        public PartnerService(IRepository<Partner> partnerRepository, ICurrentDateTimeProvider currentDateTimeProvider)
        {
            this._currentDateTimeProvider = currentDateTimeProvider;
            this._partnerRepository = partnerRepository;
        }
        public async Task CancelPartnerPromoCodeLimitAsync(Partner partner)
        {
            var activeLimit = partner.PartnerLimits.FirstOrDefault(x =>
             !x.CancelDate.HasValue);

            if (activeLimit != null)
            {
                activeLimit.CancelDate = _currentDateTimeProvider.CurrentDateTime;
            }

            await _partnerRepository.UpdateAsync(partner);
        }

        public async Task<IEnumerable<Partner>> GetAllPartnersAsync()
        {
            var partners = await _partnerRepository.GetAllAsync();
            return partners;
        }

        public async Task<Partner> GetPartnerAsync(Guid id)
        {
            var partner = await _partnerRepository.GetByIdAsync(id);
            return partner;
        }

        public PartnerPromoCodeLimit GetPartnerPromoCodeLimit(Partner partner, Guid limitId)
        {
            var partnerLimit = partner.PartnerLimits.First(l => l.Id == limitId);
            return partnerLimit;
        }

        public async Task<PartnerPromoCodeLimit> SetPartnerPromoCodeLimitAsync(int limit, DateTime endDate, Partner partner)
        {
            var activeLimits = partner.PartnerLimits.Where(x => !x.CancelDate.HasValue);
            if (activeLimits.Count() > 0)
                partner.NumberIssuedPromoCodes = 0;

            foreach (PartnerPromoCodeLimit activeLimit in activeLimits)
                activeLimit.CancelDate = _currentDateTimeProvider.CurrentDateTime;

            var newLimit = new PartnerPromoCodeLimit()
            {
                Limit = limit,
                Partner = partner,
                PartnerId = partner.Id,
                CreateDate = DateTime.Now,
                EndDate = endDate
            };

            partner.PartnerLimits.Add(newLimit);
            await _partnerRepository.UpdateAsync(partner);
            return newLimit;
        }
    }
}

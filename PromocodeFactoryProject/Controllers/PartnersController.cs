using Core.Domain;
using Core.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PromocodeFactoryProject.ErrorHandling;
using PromocodeFactoryProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromocodeFactoryProject.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PartnersController : ControllerBase
    {
        readonly IRepository<Partner> _partnerRepository;
        readonly ICurrentDateTimeProvider _currentDateTimeProvider;

        public PartnersController(IRepository<Partner> partnerRepository, ICurrentDateTimeProvider currentDateTimeProvider)
        {
            this._currentDateTimeProvider = currentDateTimeProvider;
            this._partnerRepository = partnerRepository;
        }

        ///<summary>
        /// Get all partners
        ///</summary>
        [HttpGet]
        public async Task<ActionResult<List<PartnerResponse>>> GetPartnersAsync()
        {
            var partners = await _partnerRepository.GetAllAsync();
            var partnerModelList = partners.Select(x => PartnerToDTO(x));
            return Ok(partnerModelList);
        }

        private PartnerResponse PartnerToDTO(Partner x)
        {
            PartnerResponse model = new PartnerResponse()
            {
                Id = x.Id,
                Name = x.Name,
                NumberIssuedPromoCodes = x.NumberIssuedPromoCodes,
                IsActive = x.IsActive,
                PartnerLimits = x.PartnerLimits.Select(partnerLimit => PromoCodeLimitToDTO(partnerLimit)).ToList()

            };
            return model;
        }
        private PartnerPromoCodeLimitResponse PromoCodeLimitToDTO(PartnerPromoCodeLimit partnerLimit)
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

        ///<summary>
        /// get a promocode limit for the specified partner
        ///</summary>
        [HttpGet("{id}/limits/{limitId}")]
        public async Task<ActionResult<PartnerPromoCodeLimitResponse>> GetPartnerLimitAsync(Guid id, Guid limitId)
        {
            var partner = await _partnerRepository.GetByIdAsync(id);
            if (partner == null)
            {
                var error = new HttpResponseException()
                {
                    Value = "Cannot find a partner with this Id",
                    Status = StatusCodes.Status404NotFound
                };
                throw error;
                // return NotFound();
            }

            var partnerLimit = partner.PartnerLimits.First(l => l.Id == limitId);
            if (partnerLimit == null)
            {
                var error = new HttpResponseException()
                {
                    Value = "Cannot find a promocode limit for this partner",
                    Status = StatusCodes.Status404NotFound
                };
                throw error;
            }
            var limitResponse = PromoCodeLimitToDTO(partnerLimit);
            return Ok(limitResponse);
        }
        ///<summary>
        /// set a promocode limit for the specified partner
        ///</summary>
        ///<example>
        ///{
        ///"id": "0da65561-cf56-4942-bff2-22f50cf70d43",
        ///  "endDate": "2022-08-09T09:44:48.372Z",
        ///  "limit": 3
        ///}
        ///</example>
        [HttpPost("{id}/limits")]
        public async Task<IActionResult> SetPartnerPromoCodeLimitAsync(Guid id, PartnerPromoCodeLimitRequest request)
        {
            var partner = await _partnerRepository.GetByIdAsync(id);
            if (partner == null)
            {
                var error = new HttpResponseException()
                {
                    Value = "Cannot find a partner with this Id",
                    Status = StatusCodes.Status404NotFound
                };
                throw error;
                // return NotFound();
            }

            if (!partner.IsActive)
            {
                return BadRequest("The partner is disabled!!");
            }

            var activeLimits = partner.PartnerLimits.Where(x => !x.CancelDate.HasValue);
            if (activeLimits.Count() > 0)
                partner.NumberIssuedPromoCodes = 0;

            foreach (PartnerPromoCodeLimit activeLimit in activeLimits)
                activeLimit.CancelDate = _currentDateTimeProvider.CurrentDateTime;

            if (request.Limit <= 0)
                return BadRequest("Limit should be larger than 0");


            var newLimit = new PartnerPromoCodeLimit()
            {
                Limit = request.Limit,
                Partner = partner,
                PartnerId = partner.Id,
                CreateDate = DateTime.Now,
                EndDate = request.EndDate
            };

            partner.PartnerLimits.Add(newLimit);
            await _partnerRepository.UpdateAsync(partner);
            return CreatedAtAction(nameof(GetPartnerLimitAsync), new { id = partner.Id, limitId = newLimit.Id }, null);
        }
        ///<summary>
        /// Reset the promocode limit for the specified partner
        ///</summary>
        [HttpPost("{id}/canceledLimits")]
        public async Task<IActionResult> CancelPartnerPromoCodeLimitAsync(Guid id)
        {
            var partner = await _partnerRepository.GetByIdAsync(id);
            if (partner == null)
            {
                var error = new HttpResponseException()
                {
                    Value = "Cannot find a partner with this Id",
                    Status = StatusCodes.Status404NotFound
                };
                throw error;
                //   return NotFound();
            }

            if (!partner.IsActive)
                return BadRequest("The partner is disabled!!");

            var activeLimit = partner.PartnerLimits.FirstOrDefault(x =>
               !x.CancelDate.HasValue);

            if (activeLimit != null)
            {
                activeLimit.CancelDate = _currentDateTimeProvider.CurrentDateTime;
            }

            await _partnerRepository.UpdateAsync(partner);

            return NoContent();

        }
    }
}

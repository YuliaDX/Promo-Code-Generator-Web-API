using BusinessLogic.Abstractions;
using Core.Domain;
using Core.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PromocodeFactoryProject.ErrorHandling;
using PromocodeFactoryProject.Mappers;
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
        private readonly IPartnerService _partnerService;
        readonly IPartnerMapper _partnerMapper;

        public PartnersController(IPartnerService partnerService, IPartnerMapper partnerMapper)
        {
            this._partnerMapper = partnerMapper;
            _partnerService = partnerService;
        }

        ///<summary>
        /// Get all partners
        ///</summary>
        [HttpGet]
        public async Task<ActionResult<List<PartnerResponse>>> GetPartnersAsync()
        {
            var partners = await _partnerService.GetAllPartnersAsync();
            var partnerModelList = partners.Select(x => _partnerMapper.MapPartnerEntityToDTO(x));
            return Ok(partnerModelList);
        }

        ///<summary>
        /// get a promocode limit for the specified partner
        ///</summary>
        [HttpGet("{id}/limits/{limitId}")]
        public async Task<ActionResult<PartnerPromoCodeLimitResponse>> GetPartnerLimitAsync(Guid id, Guid limitId)
        {
            var partner = await _partnerService.GetPartnerAsync(id);
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

            var partnerLimit = _partnerService.GetPartnerPromoCodeLimit(partner, limitId);
            if (partnerLimit == null)
            {
                var error = new HttpResponseException()
                {
                    Value = "Cannot find a promocode limit for this partner",
                    Status = StatusCodes.Status404NotFound
                };
                throw error;
            }
            var limitResponse =_partnerMapper.MapPromoCodeLimitToDTO(partnerLimit);
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
            var partner = await _partnerService.GetPartnerAsync(id);
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
            if (request.Limit <= 0)
                return BadRequest("Limit should be larger than 0");

            if (!partner.IsActive)
            {
                return BadRequest("The partner is disabled!!");
            }

            var newLimit = await _partnerService.SetPartnerPromoCodeLimitAsync(request.Limit, request.EndDate, partner);
           
            return CreatedAtAction(nameof(GetPartnerLimitAsync), new { id = partner.Id, limitId = newLimit.Id }, null);
        }
        ///<summary>
        /// Reset the promocode limit for the specified partner
        ///</summary>
        ///     ///<example>
        ///{
        ///"id": "894b6e9b-eb5f-406c-aefa-8ccb35d39319"
        ///}
        ///</example>
        [HttpPost("{id}/canceledLimits")]
        public async Task<IActionResult> CancelPartnerPromoCodeLimitAsync(Guid id)
        {
            var partner = await _partnerService.GetPartnerAsync(id);
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

            await _partnerService.CancelPartnerPromoCodeLimitAsync(partner);

            return NoContent();

        }
    }
}

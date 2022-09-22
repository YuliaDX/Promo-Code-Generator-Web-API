using BusinessLogic.Abstractions;
using Core;
using Core.Domain;
using Core.Repositories;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;
using PromocodeFactoryProject.Mappers;
using PromocodeFactoryProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PromocodeFactoryProject.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PromocodesController
         : ControllerBase
    {
        private readonly IPromoCodeService _promoCodeService;
        readonly IPromoCodeMapper _promoCodeMapper;
        
        public PromocodesController(IPromoCodeService promoCodeService, IPromoCodeMapper promoCodeMapper)
        {
            this._promoCodeService = promoCodeService;
            this._promoCodeMapper = promoCodeMapper;
           
        }
        /// <summary>
        /// Get all promocodes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<PromoCodeResponse>>> GetPromocodesAsync()
        {
            var promoCodes = await _promoCodeService.GetAllPromoCodesAsync();
            var promocodesModel = promoCodes.Select(x => _promoCodeMapper.MapPromocodeToDTO(x)) as IList<PromoCodeResponse>;
            return Ok(promocodesModel);
        }
   
        /// <summary>
        /// Create a promocode and send it to customers with the specified preference
        /// </summary>
        /// <returns></returns>
        ///<example>
        ///{
        ///"PartnerName":"SBER",
        ///"PromoCode":"CHEAP",
        ///"PartnerManagerId":"f766e2bf-340a-46ea-bff3-f1700b435895",
        ///"Preference": "Cinema"
        ///}
        ///</example>
        [HttpPost]
        public async Task<IActionResult> GivePromoCodesToCustomersWithPreferenceAsync(PromoCodeRequest request)
        {
            var preference = await _promoCodeService.GetPreferenceAsync(request.Preference);
            var employee = await _promoCodeService.GetPartnerManagerAsync(request.PartnerManagerId);
            var promoCode = _promoCodeMapper.MapPromoCodeFromModel(request, preference, employee);

            await _promoCodeService.CreatePromoCodeAsync(promoCode);

            await _promoCodeService.GivePromoCodesToCustomersAsync(promoCode);
            return NoContent();
        }
    }

}

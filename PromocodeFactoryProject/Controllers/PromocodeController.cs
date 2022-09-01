using Core.Domain;
using Core.Repositories;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;
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
    public class PromocodesController
         : ControllerBase
    {
        readonly IRepository<PromoCode> _promocodeRepository;
        readonly IRepository<Preference> _preferenceRepository;
        readonly IPromoCodeMapper _promoCodeMapper;
        readonly CustomerRepository _customerRepository;
        public PromocodesController(IRepository<PromoCode> promocodeRepository, 
            IRepository<Preference> preferenceRepository, IRepository<Customer> customerRepository,
            IPromoCodeMapper promoCodeMapper)
        {
            this._customerRepository = (CustomerRepository)customerRepository;
            this._promoCodeMapper = promoCodeMapper;
            this._preferenceRepository = preferenceRepository;
            this._promocodeRepository = promocodeRepository;
        }
        /// <summary>
        /// Get all promocodes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<PromoCodeResponse>>> GetPromocodesAsync()
        {
            var promocodes = await _promocodeRepository.GetAllAsync();
            var promocodesModel = promocodes.Select(x => PromocodeToDTO(x)) as IList<PromoCodeResponse>;
            return Ok(promocodesModel);
        }
        private static PromoCodeResponse PromocodeToDTO(PromoCode promoCode) =>
      new PromoCodeResponse()
      {
          Id = promoCode.Id,
          Code = promoCode.Code,
          ServiceInfo = promoCode.ServiceInfo,
          BeginDate = promoCode.BeginDate.ToString("yyyy-MM-dd"),
          EndDate = promoCode.EndDate.ToString("yyyy-MM-dd"),
          PartnerName = promoCode.PartnerName
          
      };
        /// <summary>
        /// Create a promocode and send it to customers with the specified preference
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GivePromoCodesToCustomersWithPreferenceAsync(PromoCodeRequest request)
        {
            var preferences = await _preferenceRepository.GetAllAsync();
            var promoCode = _promoCodeMapper.MapFromModel(request, preferences);
            await _promocodeRepository.AddAsync(promoCode);
            IEnumerable<Customer> customers = await _customerRepository.GetByCondition(customer =>
            customer.Preferences.Any(p => p.PreferenceId == promoCode.Preference.Id));
            foreach (Customer customer in customers)
                customer.PromoCodes.Add(promoCode);

            await _customerRepository.SaveAsync();
            return NoContent();
        }
    }

}

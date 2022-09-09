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
        readonly IRepository<PromoCode> _promocodeRepository;
        readonly IRepository<Preference> _preferenceRepository;
        private readonly IRepository<Employee> _employeeRepository;
        readonly IPromoCodeMapper _promoCodeMapper;
        readonly CustomerRepository _customerRepository;
        public PromocodesController(IRepository<PromoCode> promocodeRepository, 
            IRepository<Preference> preferenceRepository, IRepository<Customer> customerRepository,
          IRepository<Employee> employeeRepository,  IPromoCodeMapper promoCodeMapper)
        {
            this._customerRepository = (CustomerRepository)customerRepository;
            this._promoCodeMapper = promoCodeMapper;
            this._preferenceRepository = preferenceRepository;
            this._employeeRepository = employeeRepository;
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
            var preferences = await _preferenceRepository.GetAllAsync();
            var employees = await _employeeRepository.GetAllAsync();
            var promoCode = _promoCodeMapper.MapFromModel(request, preferences, employees);
            await _promocodeRepository.AddAsync(promoCode);

            Expression<Func<Customer, bool>> expression = customer =>
                customer.Preferences.Any(p => p.PreferenceId == promoCode.Preference.Id);

            IEnumerable<Customer> customers = await _customerRepository.GetByConditionAsync(expression);
            foreach (Customer customer in customers)
                customer.PromoCodes.Add(promoCode);

            await _customerRepository.SaveAsync();
            return NoContent();
        }
    }

}

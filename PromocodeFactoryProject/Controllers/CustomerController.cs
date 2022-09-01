using Core.Domain;
using Core.Repositories;
using DataAccess;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public class CustomersController
      : ControllerBase
    {
        readonly IRepository<Customer> _eFRepository;
        readonly IRepository<Preference> _preferenceRepository;
        readonly ICustomerMapper _customerMapper;
        public CustomersController(IRepository<Customer> customerRepository, IRepository<Preference> preferenceRepository,
            ICustomerMapper customerMapper)
        {
            this._customerMapper = customerMapper;
            this._preferenceRepository = preferenceRepository;
            this._eFRepository = customerRepository;
        }
        ///<summary>
        /// Get all customers
        ///</summary>
        [HttpGet]
        public async Task<ActionResult<CustomerShortResponse>> GetCustomersAsync()
        {
            var customers = await _eFRepository.GetAllAsync();
            var customerModelList = customers.Select(x => CustomerToShortDTO(x));
            return Ok(customerModelList);
        }
        private static CustomerShortResponse CustomerToShortDTO(Customer customer) =>
            new CustomerShortResponse()
            {
                Id = customer.Id,
                Email = customer.Email,
                FirstName = customer.FirstName,
                LastName = customer.LastName
            };
        private static CustomerResponse CustomerToDTO(Customer customer) =>
            new CustomerResponse()
            {
                Id = customer.Id,
                Email = customer.Email,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Preferences = customer.Preferences.Select(p=> new PreferenceResponse()
                {
                    Id = p.PreferenceId,
                    Name = p.Preference.Name

                }).ToList()
            };
        ///<summary>
        /// Get a customer with the specified Id
        ///</summary>
        ///<example>
        ///{
        ///"id": "151533d5-d8d5-4a11-9c7b-eb9f14e1a32f"
        ///}
        ///</example>
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerResponse>> GetCustomerAsync(Guid id)
        {
            var customer = await _eFRepository.GetByIdAsync(id);

            if (customer == null)
            {
                var error = new HttpResponseException()
                {
                    Value = "Cannot find a customer with this Id",
                    Status = StatusCodes.Status404NotFound
                };
                throw error;
                // return NotFound();
            }

            CustomerResponse customerModel = CustomerToDTO(customer);
            return Ok(customerModel);
        }
        ///<summary>
        ///Create a customer
        ///</summary>
        [HttpPost]
        public async Task<IActionResult> CreateCustomerAsync(CreateOrEditCustomerRequest request)
        {
            var preferences = await GetPreferencesAsync(request.PreferenceIds);
            var customer = _customerMapper.MapFromModel(request, preferences);
            await _eFRepository.AddAsync(customer);
            return CreatedAtAction(
              nameof(GetCustomerAsync),
              new { id = customer.Id },
              null);
        }
        private Task<IEnumerable<Preference>> GetPreferencesAsync(List<Guid> ids)
        {
            IEnumerable<Preference> preferences = new List<Preference>();
            if (ids != null && ids.Count> 0)
            {
                return _preferenceRepository.GetByCondition(x => ids.Any(item => item == x.Id));
            }

            return Task.FromResult(preferences);
        }

        ///<summary>
        /// Edit customer with the specified Id
        ///</summary>
        ///<example>
        ///{
        ///"id": "151533d5-d8d5-4a11-9c7b-eb9f14e1a32f"
        ///}
        ///</example>
        [HttpPut("{id}")]
        public async Task<IActionResult> EditCustomersAsync(Guid id, CreateOrEditCustomerRequest request)
        {
            var customer = await _eFRepository.GetByIdAsync(id);
            var preferences = await GetPreferencesAsync(request.PreferenceIds);
            if (customer == null)
            {
                //return NotFound();
                var error = new HttpResponseException(){
                    Value = "Cannot find a customer with this Id",
                    Status = StatusCodes.Status404NotFound
                };
                throw error;
            }

            if (id != customer.Id)
            {
                return BadRequest("This customer cannot be modified");

            }
            customer = _customerMapper.MapFromModel(request, preferences, customer);
            await _eFRepository.UpdateAsync(customer);
            return NoContent();
        }

        ///<summary>
        /// Delete the customer with the specified Id
        ///</summary>
        ///<example>
        ///{
        ///"id": "151533d5-d8d5-4a11-9c7b-eb9f14e1a32f"
        ///}
        ///</example>
        [HttpDelete]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            var customer = await _eFRepository.GetByIdAsync(id);
            if (customer == null)
            {
                //return NotFound();
                var error = new HttpResponseException()
                {
                    Value = "Cannot find a customer with this Id",
                    Status = StatusCodes.Status404NotFound
                };
                throw error;
            }
            await _eFRepository.RemoveAsync(customer);
            return NoContent();
        }
    }

}

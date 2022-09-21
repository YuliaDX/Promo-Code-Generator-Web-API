using BusinessLogic;
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
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PromocodeFactoryProject.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CustomersController
      : ControllerBase
    {
        readonly ICustomerMapper _customerMapper;
        readonly ICustomerService _customerService;
        public CustomersController(ICustomerService customerService,
            ICustomerMapper customerMapper)
        {
            this._customerService = customerService;
            this._customerMapper = customerMapper;
          
        }
        ///<summary>
        /// Get all customers
        ///</summary>
        [HttpGet]
        public async Task<ActionResult<CustomerShortResponse>> GetCustomersAsync()
        {
            var customers = await _customerService.GetAllCustomersAsync();
            var customerModelList = customers.Select(x => _customerMapper.MapCustomerEntityToShortDTO(x));
            return Ok(customerModelList);
        }
   
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
            var customer = await _customerService.GetCustomerAsync(id);
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
            CustomerResponse customerModel = _customerMapper.MapCustomerEntityToDTO(customer);
            return Ok(customerModel);
        }
        ///<summary>
        ///Create a customer
        ///</summary>
        ///<example>
        ///{
        ///"FirstName":"New",
        ///"LastName":"New",
        ///"Email":"some@mail.ru"
        ///"Preferences": ["ef7f299f-92d7-459f-896e-eb9f14e1a32f"]
        ///}
        ///</example>
        [HttpPost]
        public async Task<IActionResult> CreateCustomerAsync(CreateOrEditCustomerRequest request)
        {
            var preferences = await _customerService.GetPreferencesAsync(request.PreferenceIds);
            var customer = _customerMapper.MapDTOToCustomerEntity(request, preferences);
            await _customerService.CreateCustomerAsync(customer);

            return CreatedAtAction(
              nameof(GetCustomerAsync),
              new { id = customer.Id },
              null);
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
            var customer = await _customerService.GetCustomerAsync(id);
            var preferences = await _customerService.GetPreferencesAsync(request.PreferenceIds);
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
            customer = _customerMapper.MapDTOToCustomerEntity(request, preferences, customer);
            await _customerService.UpdateCustomer(customer);
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
            var customer = await _customerService.GetCustomerAsync(id);
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

            await _customerService.DeleteCustomer(customer);
            return NoContent();
        }
    }

}

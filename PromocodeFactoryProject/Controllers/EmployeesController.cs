using Core;
using Core.Repositories;
using DataAccess;
using DataAccess.Repositories;
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
using BusinessLogic.Abstractions;

namespace PromocodeFactoryProject.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EmployeesController
       : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        private readonly IEmployeeMapper _employeeMapper;
        public EmployeesController(IEmployeeService employeeService,
             IEmployeeMapper employeeMapper)
        {
            _employeeService = employeeService;
             _employeeMapper = employeeMapper;
        }

        ///<summary>
        /// Get all employees
        ///</summary>
        [HttpGet]
        public async Task<List<EmployeeResponse>> GetEmployeesAsync()
        {
            var employees = await _employeeService.GetAllEmployeesAsync();

            var employeesModelList = employees.Select(x => _employeeMapper.MapEmployeeToDTO(x)).ToList();

            return employeesModelList;
        }
      
  
        ///<summary>
        /// Get an employee by Id
        ///</summary>
        ///<example>
        ///{
        ///"id": "451533d5-d8d5-4a11-9c7b-eb9f14e1a32f"
        ///}
        ///</example>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<EmployeeResponse>> GetEmployeeByIdAsync(Guid id)
        {
            var employee = await _employeeService.GetEmployeeAsync(id);

            if (employee == null)
            {
                var error = new HttpResponseException()
                {
                    Value = "Cannot find an employee with this Id",
                    Status = StatusCodes.Status404NotFound
                };
                throw error;
                //return NotFound();
            }

            EmployeeResponse employeeModel = _employeeMapper.MapEmployeeToDTO(employee);
            return employeeModel;
        }
        ///<summary>
        /// create an employee
        ///</summary>
        [HttpPost]
        public async Task<ActionResult> CreateEmployeeAsync(CreateOrEditEmployeeRequest model)
        {
            var employee = await _employeeMapper.MapEmployeeFromModelAsync(model);
            await _employeeService.CreateEmployeeAsync(employee);

            return CreatedAtAction(
               nameof(GetEmployeeByIdAsync),
               new { id = employee.Id },
               null);
        }
        ///<summary>
        /// edit an employees by Id
        ///</summary>
        ///<example>
        ///{
        ///"id": "451533d5-d8d5-4a11-9c7b-eb9f14e1a32f"
        ///}
        ///</example>
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<EmployeeResponse>> UpdateEmployee(Guid id, CreateOrEditEmployeeRequest model)
        {
            var employee = await _employeeService.GetEmployeeAsync(id);
            if (employee == null)
            {
                // return NotFound();
                var error = new HttpResponseException()
                {
                    Value = "Cannot find an employee with this Id",
                    Status = StatusCodes.Status404NotFound
                };
                throw error;
            }

            if (id != employee.Id)
            {
                return BadRequest("This employee cannot be modified");
            }
           
            employee = await _employeeMapper.MapEmployeeFromModelAsync(model, employee);

            await _employeeService.UpdateEmployee(employee);
            return NoContent();
        }
        ///<summary>
        /// Delete the employee with the specified Id
        ///</summary>
        ///<example>
        ///{
        ///"id": "451533d5-d8d5-4a11-9c7b-eb9f14e1a32f"
        ///}
        ///</example>
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<EmployeeResponse>> DeleteEmployee(Guid id)
        {
            var employee = await _employeeService.GetEmployeeAsync(id);
            if (employee == null)
            {
                // return NotFound();
                var error = new HttpResponseException()
                {
                    Value = "Cannot find an employee with this Id",
                    Status = StatusCodes.Status404NotFound
                };
                throw error;
            }
            await _employeeService.DeleteEmployee(employee);
            return NoContent();
        }
    }
}

using BusinessLogic.Abstractions;
using Core;
using Core.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IRepository<Employee> _employeeRepository;
        public EmployeeService(IRepository<Employee> employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        public async Task CreateEmployeeAsync(Employee employee)
        {
            await _employeeRepository.AddAsync(employee);
        }

        public async Task DeleteEmployee(Employee employee)
        {
            await _employeeRepository.RemoveAsync(employee);
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();
            return employees;
        }

        public async Task<Employee> GetEmployeeAsync(Guid id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            return employee;
        }

        public async Task UpdateEmployee(Employee employee)
        {
            await _employeeRepository.UpdateAsync(employee);
        }
    }
}

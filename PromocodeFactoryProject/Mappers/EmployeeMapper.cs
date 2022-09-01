using Core;
using Core.Repositories;
using PromocodeFactoryProject.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromocodeFactoryProject.Mappers
{
    public class EmployeeMapper : IEmployeeMapper
    {
        private readonly IRepository<Role> _rolesRepository;
        public EmployeeMapper(IRepository<Role> rolesRepository)
        {
            _rolesRepository = rolesRepository;
        }
        public async Task<Employee> MapFromModelAsync(CreateOrEditEmployeeRequest model, Employee employee = null)
        {
            if (employee == null)
            {
                employee = new Employee();
                employee.Id = Guid.NewGuid();
            }

            employee.FirstName = model.FirstName;
            employee.LastName = model.LastName;
            employee.AppliedPromocodesCount = model.AppliedPromocodesCount;
            employee.Email = model.Email;

            employee.Roles = await _rolesRepository.GetByCondition(x=> model.Roles.Any(item => item.Name == x.Name)) as List<Role>;
            return employee;
            
        }
    }
}

using Core;
using Core.Repositories;
using PromocodeFactoryProject.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

            Expression<Func<Role, bool>> expression = item => item.Name == model.Role.Name;

            var roleCollection = await _rolesRepository.GetByConditionAsync(expression);
            employee.Role = ((List<Role>)roleCollection)[0];
            return employee;
            
        }
    }
}

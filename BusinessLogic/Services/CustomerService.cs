using Core.Domain;
using Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class CustomerService : ICustomerService
    {
        readonly IRepository<Preference> _preferenceRepository;
        readonly IRepository<Customer> _eFRepository;

        public CustomerService(IRepository<Customer> customerRepository, IRepository<Preference> preferenceRepository)
        {
            this._preferenceRepository = preferenceRepository;
            this._eFRepository = customerRepository;
        }

        public async Task CreateCustomerAsync(Customer customer)
        {
            await _eFRepository.AddAsync(customer);
        }

        public async Task DeleteCustomer(Customer customer)
        {
            await _eFRepository.RemoveAsync(customer);
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            var customers = await _eFRepository.GetAllAsync();
            return customers;
        }

        public async Task<Customer> GetCustomerAsync(Guid id)
        {
            var customer = await _eFRepository.GetByIdAsync(id);
            return customer;
        }

        public Task<IEnumerable<Preference>> GetPreferencesAsync(List<Guid> ids)
        {
            IEnumerable<Preference> preferences = new List<Preference>();
            if (ids != null && ids.Count > 0)
            {
                Expression<Func<Preference, bool>> expression = x => ids.Any(item => item == x.Id);
                return _preferenceRepository.GetByConditionAsync(expression);
            }

            return Task.FromResult(preferences);
        }

        public async Task UpdateCustomer(Customer customer)
        {
            await _eFRepository.UpdateAsync(customer);
        }
    }
}


using Core.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetAllCustomersAsync();
        Task<Customer> GetCustomerAsync(Guid id);
        Task CreateCustomerAsync(Customer customer);
        Task<IEnumerable<Preference>> GetPreferencesAsync(List<Guid> ids);
        Task UpdateCustomer(Customer customer);
        Task DeleteCustomer(Customer customer);
    }
}

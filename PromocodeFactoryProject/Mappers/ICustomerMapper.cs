using Core.Domain;
using Core.Repositories;
using PromocodeFactoryProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromocodeFactoryProject.Mappers
{
    public interface ICustomerMapper
    {
        public Customer MapFromModel(CreateOrEditCustomerRequest model, IEnumerable<Preference> preferences, 
            Customer customer = null);
    }
    public class CustomerMapper : ICustomerMapper
    {
        public Customer MapFromModel(CreateOrEditCustomerRequest model, IEnumerable<Preference> preferences,
            Customer customer = null)
        {
            if (customer == null)
            {
                customer = new Customer();
                customer.Id = Guid.NewGuid();
            }
            customer.FirstName = model.FirstName;
            customer.LastName = model.LastName;
            customer.Email = model.Email;
            customer.Preferences = preferences.Select(x => new CustomerPreference()
            {
                Customer = customer,
                Preference = x
            }).ToList();
            return customer;
        }
    }
}

using Core.Domain;
using PromocodeFactoryProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PromocodeFactoryProject.Mappers
{
    public class CustomerMapper : ICustomerMapper
    {
        public Customer MapDTOToCustomerEntity(CreateOrEditCustomerRequest model, IEnumerable<Preference> preferences,
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
        public CustomerShortResponse MapCustomerEntityToShortDTO(Customer customer) =>
       new CustomerShortResponse()
       {
           Id = customer.Id,
           Email = customer.Email,
           FirstName = customer.FirstName,
           LastName = customer.LastName
       };
        public  CustomerResponse MapCustomerEntityToDTO(Customer customer) =>
            new CustomerResponse()
            {
                Id = customer.Id,
                Email = customer.Email,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Preferences = customer.Preferences.Select(p => new PreferenceResponse()
                {
                    Id = p.PreferenceId,
                    Name = p.Preference.Name

                }).ToList()
            };
    }
}

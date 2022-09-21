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
        public Customer MapDTOToCustomerEntity(CreateOrEditCustomerRequest model, IEnumerable<Preference> preferences, 
            Customer customer = null);
        public CustomerShortResponse MapCustomerEntityToShortDTO(Customer customer);
        public CustomerResponse MapCustomerEntityToDTO(Customer customer);
    }
}

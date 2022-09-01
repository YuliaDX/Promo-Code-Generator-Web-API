using System;
using System.Collections.Generic;
using System.Linq;

namespace PromocodeFactoryProject.Model
{
    public class CreateOrEditCustomerRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public List<Guid> PreferenceIds { get; set; }
    }
}

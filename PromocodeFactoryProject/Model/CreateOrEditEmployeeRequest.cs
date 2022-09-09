using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromocodeFactoryProject.Model
{
    public class CreateOrEditEmployeeRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public RoleItemResponse Role { get; set; }

        public int AppliedPromocodesCount { get; set; }
    }
}

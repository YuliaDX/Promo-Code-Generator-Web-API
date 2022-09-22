using Core;
using PromocodeFactoryProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromocodeFactoryProject.Mappers
{
    public interface IRoleMapper
    {
        RoleItemResponse MapRoleToDTO(Role x);
    }
    public class RoleMapper : IRoleMapper
    {
        public RoleItemResponse MapRoleToDTO(Role x) =>
              new RoleItemResponse()
              {
                  Name = x.Name,
                  Description = x.Description
              };

    }
}

using Core;
using Core.Repositories;
using Microsoft.AspNetCore.Mvc;
using PromocodeFactoryProject.Mappers;
using PromocodeFactoryProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromocodeFactoryProject.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class RolesController
    {
        private readonly IRepository<Role> _rolesRepository;
        readonly IRoleMapper _roleMapper;

        public RolesController(IRepository<Role> rolesRepository, IRoleMapper roleMapper)
        {
            this._roleMapper = roleMapper;
            _rolesRepository = rolesRepository;
        }

        ///<summary>
        /// Get all roles
        ///</summary>
        [HttpGet]
        public async Task<List<RoleItemResponse>> GetRolesAsync()
        {
            var roles = await _rolesRepository.GetAllAsync();

            var rolesModelList = roles.Select(x => _roleMapper.MapRoleToDTO(x)).ToList();

            return rolesModelList;
        }
    }
}

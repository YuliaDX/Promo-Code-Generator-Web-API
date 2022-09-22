using Core.Domain;
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
    public class PreferenceController:ControllerBase
    {
        readonly IRepository<Preference> _preferenceRepository;
        readonly IPreferenceMapper _preferenceMapper;
        public PreferenceController(IRepository<Preference> preferenceRepository, IPreferenceMapper preferenceMapper)
        {
            this._preferenceRepository = preferenceRepository;
            _preferenceMapper = preferenceMapper;
        }
        /// <summary>
        /// get preferences
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<PreferenceResponse>> GetPreferencesAsync()
        {
            var preferences = await _preferenceRepository.GetAllAsync();

            var preferenceModelList = preferences.Select(x => _preferenceMapper.PreferenceToDTO(x)).ToList();

            return preferenceModelList;
        }
    

    }
}

using Core.Domain;
using Core.Repositories;
using Microsoft.AspNetCore.Mvc;
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
        public PreferenceController(IRepository<Preference> preferenceRepository)
        {
            this._preferenceRepository = preferenceRepository;
        }
        /// <summary>
        /// get preferences
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<PreferenceResponse>> GetPreferencesAsync()
        {
            var preferences = await _preferenceRepository.GetAllAsync();

            var preferenceModelList = preferences.Select(x => PreferenceToDTO(x)).ToList();

            return preferenceModelList;
        }
        private static PreferenceResponse PreferenceToDTO(Preference preference) =>
          new PreferenceResponse()
          {
              Id = preference.Id,
              Name = preference.Name
          };


    }
}

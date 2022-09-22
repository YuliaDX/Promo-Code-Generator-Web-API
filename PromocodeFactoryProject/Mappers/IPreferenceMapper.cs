using Core.Domain;
using PromocodeFactoryProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromocodeFactoryProject.Mappers
{
    public interface IPreferenceMapper
    {
        PreferenceResponse PreferenceToDTO(Preference preference);
    }

    public class PreferenceMapper : IPreferenceMapper
    {
        public PreferenceResponse PreferenceToDTO(Preference preference) =>
           new PreferenceResponse()
           {
               Id = preference.Id,
               Name = preference.Name
           };

    }
}

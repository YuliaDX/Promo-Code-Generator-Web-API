using BusinessLogic.Abstractions;
using Core;
using Core.Domain;
using Core.Repositories;
using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class PromoCodeService : IPromoCodeService
    {
        readonly IRepository<PromoCode> _promocodeRepository;
        readonly IRepository<Preference> _preferenceRepository;
        readonly IRepository<Employee> _employeeRepository;
        readonly CustomerRepository _customerRepository;

        public PromoCodeService(IRepository<PromoCode> promocodeRepository,
            IRepository<Preference> preferenceRepository, IRepository<Customer> customerRepository,
          IRepository<Employee> employeeRepository)
        {
            this._preferenceRepository = preferenceRepository;
            this._employeeRepository = employeeRepository;
            this._promocodeRepository = promocodeRepository;
            this._customerRepository = (CustomerRepository)customerRepository;
        }
        public async Task CreatePromoCodeAsync(PromoCode promoCode)
        {
            await _promocodeRepository.AddAsync(promoCode);
        }

        public async Task<IEnumerable<PromoCode>> GetAllPromoCodesAsync()
        {
            var promocodes = await _promocodeRepository.GetAllAsync();
            return promocodes;
        }
        public async Task<Preference> GetPreferenceAsync(string preference)
        {
            var preferences = await _preferenceRepository.GetAllAsync();
            return preferences.FirstOrDefault(p => p.Name == preference);
        }
        public async Task<Employee> GetPartnerManagerAsync(Guid partnerManagerId)
        {
            var employees = await _employeeRepository.GetAllAsync();
            return employees.FirstOrDefault(e => e.Id == partnerManagerId);
        }
        public async Task GivePromoCodesToCustomersAsync(PromoCode promoCode)
        {
            Expression<Func<Customer, bool>> expression = customer =>
                customer.Preferences.Any(p => p.PreferenceId == promoCode.Preference.Id);

            IEnumerable<Customer> customers = await _customerRepository.GetByConditionAsync(expression);
            foreach (Customer customer in customers)
                customer.PromoCodes.Add(promoCode);

            await _customerRepository.SaveAsync();
        }
    }
}

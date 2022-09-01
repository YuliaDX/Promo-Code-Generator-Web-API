using Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class CustomerRepository : EFRepository<Customer>
    {
        readonly DataContext _dataContext;
        public CustomerRepository(DataContext dataContext):base(dataContext)
        {
            this._dataContext = dataContext;
        }
        public async Task SaveAsync()
        {
            await _dataContext.SaveChangesAsync();
        }
    }
}

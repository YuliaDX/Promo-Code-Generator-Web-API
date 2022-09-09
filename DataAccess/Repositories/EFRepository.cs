using Core;
using Core.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccess.Repositories
{
    public class EFRepository<T> : IRepository<T>
         where T : BaseEntity
    {
        readonly DataContext _dataContext;
        public EFRepository(DataContext dataContext)
        {
            this._dataContext = dataContext;
        }
        public async Task AddAsync(T item)
        {
           await _dataContext.Set<T>().AddAsync(item);
           await _dataContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
           var entities = await _dataContext.Set<T>().ToListAsync();
           return entities;
        }

        public async Task<IEnumerable<T>> GetByConditionAsync(Expression<Func<T, bool>> predicate)
        {
            var entities = await _dataContext.Set<T>().Where(predicate).ToListAsync();
            return entities;

        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            var entity = await _dataContext.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
            return entity;
        }

        public async Task RemoveAsync(T item)
        {
            _dataContext.Set<T>().Remove(item);
            await _dataContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(T item)
        {
           await _dataContext.SaveChangesAsync();
        }
    }
}

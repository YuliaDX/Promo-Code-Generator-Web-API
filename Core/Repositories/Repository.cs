using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public interface IRepository<T>
        where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetByConditionAsync(Expression<Func<T, bool>> predicate);

        Task<T> GetByIdAsync(Guid id);
        Task RemoveAsync(T item);
        Task AddAsync(T item);
        Task UpdateAsync(T item);
    }
}

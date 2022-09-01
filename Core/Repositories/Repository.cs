using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public interface IRepository<T>
        where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetByCondition(Func <T, bool> predicate);

        Task<T> GetByIdAsync(Guid id);
        Task RemoveAsync(T item);
        Task AddAsync(T item);
        Task UpdateAsync(T item);
    }
}

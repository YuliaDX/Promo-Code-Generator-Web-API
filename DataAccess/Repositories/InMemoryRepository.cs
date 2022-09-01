using Core;
using Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class InMemoryRepository<T>
         : IRepository<T>
         where T : BaseEntity
    {
        protected IEnumerable<T> Data { get; set; }

        public InMemoryRepository(IEnumerable<T> data)
        {
            Data = data;
        }

        public Task<IEnumerable<T>> GetAllAsync()
        {
            return Task.FromResult(Data);
        }

        public Task<T> GetByIdAsync(Guid id)
        {
            return Task.FromResult(Data.FirstOrDefault(x => x.Id == id));
        }

        public Task RemoveAsync(T item)
        {
            var data = Data as List<T>;
            data.Remove(item);
            return Task.CompletedTask;
        }

        public Task AddAsync(T item)
        {
            var data = Data as List<T>;
            data.Add(item);
            return Task.CompletedTask;
        }
        public Task UpdateAsync(T item)
        {
            var data = Data as List<T>;
            var dataObject = data.FirstOrDefault(i => i.Id == item.Id);
            data.Remove(dataObject);
            data.Add(item);
            Data = data;

            return Task.CompletedTask;
        }
        public Task<IEnumerable<T>> GetByCondition(Func<T, bool> predicate)
        {
            var data = Data as List<T>;
            return Task.FromResult(data.Where(predicate).AsEnumerable());
        }

       
    }
}

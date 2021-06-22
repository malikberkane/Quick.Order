using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Quick.Order.AppCore.Contracts.Repositories
{
    public interface IRepositoryBase<TEntity> where TEntity : class
    {
        Task<TEntity> Add(TEntity item);
        Task<TEntity> GetById(Guid id);
        Task<IEnumerable<TEntity>> Get();
        Task<bool> Update(TEntity item);
        Task<int> Count();
        Task<bool> Delete(TEntity item);
    }
}

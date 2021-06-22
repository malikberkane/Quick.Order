using Quick.Order.AppCore.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Quick.Order.AppCore.Contracts.Repositories
{
    public interface IMenuRepository : IRepositoryBase<Models.Menu>
    {
        Task<IEnumerable<Restaurant>> Get(Func<Menu, bool> func);
    }
}

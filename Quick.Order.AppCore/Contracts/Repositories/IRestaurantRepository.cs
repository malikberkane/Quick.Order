using Quick.Order.AppCore.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Quick.Order.AppCore.Contracts.Repositories
{
    public interface IRestaurantRepository : IRepositoryBase<Models.Restaurant>
    {
        Task<IEnumerable<Restaurant>> Get(Func<Restaurant, bool> func);
    }

    public interface ICacheRestaurantRepository: IRestaurantRepository
    {

    }

    public interface IRemoteRestaurantRepository : IRestaurantRepository
    {

    }
}

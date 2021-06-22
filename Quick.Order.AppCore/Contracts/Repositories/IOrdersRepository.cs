using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Quick.Order.AppCore.Contracts.Repositories
{
    public interface IOrdersRepository: IRepositoryBase<Models.Restaurant>
    {
        Task<List<Models.Restaurant>> GetOrdersForRestaurant(Guid restaurantId);

    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Quick.Order.AppCore.Contracts.Repositories
{
    public interface IOrdersRepository: IRepositoryBase<Models.Order>
    {
        Task<List<Models.Order>> GetOrdersForRestaurant(Guid restaurantId);

    }
}

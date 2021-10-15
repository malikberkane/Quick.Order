using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Quick.Order.AppCore.Contracts.Repositories
{
    public interface IOrdersRepository: IRepositoryBase<Models.Order>
    {
        Task<List<Models.Order>> GetOrdersForRestaurant(Guid restaurantId);

        event OrdersEventHandler OrderAddedOrDeleted;

        void StartOrdersObservation(Guid restaurantId);
    }


    public class OrdersEventArgs: EventArgs
    {
        public bool IsDeleted { get; set; }

        public Models.Order Order { get; set; }
    }

    public delegate void OrdersEventHandler(object source, OrdersEventArgs e);

}

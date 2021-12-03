using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Quick.Order.AppCore.Contracts.Repositories
{
    public interface IOrdersRepository: IRepositoryBase<Models.Order>
    {
        Task<List<Models.Order>> GetOrdersForRestaurant(Guid restaurantId);

        Task<IEnumerable<AppCore.Models.Order>> Get(Func<Models.Order, bool> predicate);

        event OrdersEventHandler OrderAddedOrDeleted;

        event OrdersEventHandler ObservedOrderStatusChanged;

        void StartOrdersObservation(Guid restaurantId);

        void StartOrdersStatusObservation(Guid orderId);
    }

    public class OrdersEventArgs: EventArgs
    {
        public bool IsDeleted { get; set; }

        public Models.Order Order { get; set; }
    }

    public delegate void OrdersEventHandler(object source, OrdersEventArgs e);

}

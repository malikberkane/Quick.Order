using Quick.Order.AppCore.Contracts.Repositories;
using System;

namespace Quick.Order.AppCore.BusinessOperations
{
    public class OrderStatusTrackingService
    {
        private readonly IOrdersRepository ordersRepository;

        public OrderStatusTrackingService(IOrdersRepository ordersRepository)
        {
            this.ordersRepository = ordersRepository;
        }
        public event OrderStatusChangedEventHandler OrderStatusChanged;


        public delegate void OrderStatusChangedEventHandler(object sender, OrderStatusChangedEventArgs args);


        public void StartOrderTracking(Models.Order order)
        {
            ordersRepository.ObservedOrderStatusChanged += OrdersRepository_ObservedOrderStatusChanged;
            ordersRepository.StartOrdersStatusObservation(order.Id);

        }

        private void OrdersRepository_ObservedOrderStatusChanged(object source, OrdersEventArgs e)
        {
            OrderStatusChanged.Invoke(this, new OrderStatusChangedEventArgs { UpToDateOrder = e.Order });
        }



        public void StopOrderTracking()
        {
            ordersRepository.ObservedOrderStatusChanged -= OrdersRepository_ObservedOrderStatusChanged;

        }


    }


    public class OrderStatusChangedEventArgs: EventArgs
    {
        public AppCore.Models.Order UpToDateOrder { get; set; }
    }



  






}

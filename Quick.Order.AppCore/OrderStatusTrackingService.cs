using Quick.Order.AppCore.BusinessOperations;
using Quick.Order.AppCore.Contracts.Repositories;
using System;
using System.Threading.Tasks;

namespace Quick.Order.AppCore
{
    public class OrderStatusTrackingService
    {
        private readonly FrontOfficeRestaurantService frontOfficeRestaurantService;
        private readonly IOrdersRepository ordersRepository;

        public OrderStatusTrackingService(FrontOfficeRestaurantService frontOfficeRestaurantService, IOrdersRepository ordersRepository)
        {
            this.frontOfficeRestaurantService = frontOfficeRestaurantService;
            this.ordersRepository = ordersRepository;
        }
        public event OrderStatusChangedEventHandler OrderStatusChanged;

        private AppCore.Models.Order CurrentOrder { get; set; }

        public delegate void OrderStatusChangedEventHandler(object sender, OrderStatusChangedEventArgs args);


        public void StartOrderTracking(AppCore.Models.Order order)
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

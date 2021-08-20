using Quick.Order.AppCore.BusinessOperations;
using System;
using System.Threading.Tasks;

namespace Quick.Order.AppCore
{
    public class OrderStatusTrackingService
    {
        private readonly FrontOfficeRestaurantService frontOfficeRestaurantService;

        private System.Threading.Timer Timer;
        public OrderStatusTrackingService(FrontOfficeRestaurantService frontOfficeRestaurantService)
        {
            this.frontOfficeRestaurantService = frontOfficeRestaurantService;
        }
        public event OrderStatusChangedEventHandler OrderStatusChanged;

        private AppCore.Models.Order CurrentOrder { get; set; }

        public delegate void OrderStatusChangedEventHandler(object sender, OrderStatusChangedEventArgs args);


        public void StartOrderTracking(AppCore.Models.Order order)
        {
            CurrentOrder = order;

            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromSeconds(20);
            Timer = new System.Threading.Timer(async (e) =>
            {
                await CheckOrderStatus(order);

            }, null, startTimeSpan, periodTimeSpan);
        }

        private async Task CheckOrderStatus(Models.Order order)
        {
            var upToDateOrder = await frontOfficeRestaurantService.GetOrderStatuts(order);

            if (CurrentOrder.OrderStatus != upToDateOrder.OrderStatus)
            {
                OrderStatusChanged.Invoke(this, new OrderStatusChangedEventArgs { UpToDateOrder = upToDateOrder });
                CurrentOrder.OrderStatus = upToDateOrder.OrderStatus;
            }
        }

        public void StopOrderTracking()
        {
            this.Timer.Dispose();
        }
       

    }


    public class OrderStatusChangedEventArgs: EventArgs
    {
        public AppCore.Models.Order UpToDateOrder { get; set; }
    }



  






}

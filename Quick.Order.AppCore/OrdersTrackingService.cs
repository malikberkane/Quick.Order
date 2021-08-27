using Quick.Order.AppCore.BusinessOperations;
using Quick.Order.AppCore.Contracts;
using Quick.Order.AppCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quick.Order.AppCore
{
    public class OrdersTrackingService
    {

        private System.Threading.Timer Timer;
        private readonly BackOfficeRestaurantService backOfficeService;
        private readonly BackOfficeSessionService backOfficeSessionService;
        private readonly ILoggerService loggerService;
        private readonly IConnectivityService connectivityService;

        public OrdersTrackingService(BackOfficeRestaurantService backOfficeService, BackOfficeSessionService backOfficeSessionService, ILoggerService loggerService, IConnectivityService connectivityService)
        {
            this.backOfficeService = backOfficeService;
            this.backOfficeSessionService = backOfficeSessionService;
            this.loggerService = loggerService;
            this.connectivityService = connectivityService;
        }
        public event OrdersChangedEventHandler OrderListChanged;


        public delegate void OrdersChangedEventHandler(object sender, OrdersChangedEventArgs args);

        public IEnumerable<AppCore.Models.Order> CurrentOrders { get; set; } = new List<AppCore.Models.Order>();
        public void StartOrdersTracking()
        {
            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromSeconds(20);
            Timer = new System.Threading.Timer(async (e) =>
            {
                await CheckForNewOrders();

            }, null, startTimeSpan, periodTimeSpan);
        }

        private async Task CheckForNewOrders()
        {

            try
            {
                if (!connectivityService.HasNetwork() || backOfficeSessionService.CurrentRestaurantSession==null)
                {
                    return;
                }

                var upToDateOrdersList = await backOfficeService.GetOrdersForRestaurant(backOfficeSessionService.CurrentRestaurantSession.Id);
                var comparison = CompareNewItems(upToDateOrdersList);
                if (!comparison.AreSame)
                {
                    CurrentOrders = upToDateOrdersList;
                    OrderListChanged.Invoke(this, new OrdersChangedEventArgs { ListDifferences = comparison });
                }
            }
            catch (Exception ex)
            {

                loggerService.Log(ex);
            }


        }

        public void StopOrderTracking()
        {
            this.Timer.Dispose();
        }

        public ListDifferences CompareNewItems(IEnumerable<AppCore.Models.Order> upToDateList)
        {
            return new ListDifferences
            {
                NewItems = upToDateList.Except(CurrentOrders, new OrderEqualityComparer()).ToList(),
                RemovedItems = CurrentOrders.Except(upToDateList, new OrderEqualityComparer()).ToList()

            };
        }


    }

    public class ListDifferences
    {
        public bool AreSame => !NewItems.Any() && !RemovedItems.Any();

        public List<AppCore.Models.Order> NewItems { get; set; }

        public List<AppCore.Models.Order> RemovedItems { get; set; }

    }

    public class OrdersChangedEventArgs : EventArgs
    {
        public ListDifferences ListDifferences { get; set; }
    }


}

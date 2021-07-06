using MalikBerkane.MvvmToolkit;
using Quick.Order.AppCore;
using Quick.Order.AppCore.BusinessOperations;
using Quick.Order.Native.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Quick.Order.Native.ViewModels
{
    public class WaitingForOrderPageModel : PageModelBase<WaitingForOrderParams>
    {
        private readonly OrderStatusTrackingService orderStatusTrackingService;
        private readonly FrontOfficeRestaurantService frontOfficeRestaurantService;
        private readonly INavigationService navigationService;

        public ICommand  DismissOrderTrackingCommand { get; set; } 
        public AppCore.Models.Order Order { get; set; }
        public WaitingForOrderPageModel(OrderStatusTrackingService orderStatusTrackingService, FrontOfficeRestaurantService frontOfficeRestaurantService, INavigationService navigationService)
        {
            this.orderStatusTrackingService = orderStatusTrackingService;
            this.frontOfficeRestaurantService = frontOfficeRestaurantService;
            this.navigationService = navigationService;
            DismissOrderTrackingCommand = new AsyncCommand(navigationService.GoToLanding);
        }


        public override async Task InitAsync()
        {
            Order = await frontOfficeRestaurantService.GetOrder(Parameter.OrderId);
            if (Order != null)
            {
                orderStatusTrackingService.StartOrderTracking(Order);
                orderStatusTrackingService.OrderStatusChanged += OrderStatusTrackingService_OrderStatusChanged;
            }
   
          
        }

        private void OrderStatusTrackingService_OrderStatusChanged(object sender, OrderStatusChangedEventArgs args)
        {
            Order = args.UpToDateOrder; 
        }

        public override Task CleanUp()
        {
            orderStatusTrackingService.OrderStatusChanged -= OrderStatusTrackingService_OrderStatusChanged;
            return Task.CompletedTask;
        }
    }

    public class WaitingForOrderParams
    {
        public  Guid OrderId { get; set; }
    }


}
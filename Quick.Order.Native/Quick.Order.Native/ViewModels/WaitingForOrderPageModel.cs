using MalikBerkane.MvvmToolkit;
using Quick.Order.AppCore;
using Quick.Order.AppCore.BusinessOperations;
using Quick.Order.AppCore.Contracts;
using Quick.Order.Native.Services;
using Quick.Order.Native.ViewModels.Base;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Quick.Order.Native.ViewModels
{
    public class WaitingForOrderPageModel : ExtendedPageModelBase<WaitingForOrderParams>
    {
        private readonly OrderStatusTrackingService orderStatusTrackingService;
        private readonly FrontOfficeRestaurantService frontOfficeRestaurantService;
        private readonly INavigationService navigationService;
        private readonly IVibrationService vibrationService;

        public ICommand  DismissOrderTrackingCommand { get; set; } 
        public OrderVm Order { get; set; }
        public WaitingForOrderPageModel(OrderStatusTrackingService orderStatusTrackingService, FrontOfficeRestaurantService frontOfficeRestaurantService, INavigationService navigationService, IVibrationService vibrationService)
        {
            this.orderStatusTrackingService = orderStatusTrackingService;
            this.frontOfficeRestaurantService = frontOfficeRestaurantService;
            this.navigationService = navigationService;
            this.vibrationService = vibrationService;
            DismissOrderTrackingCommand = CreateAsyncCommand(GoToLanding);
        }

        private Task GoToLanding()
        {
            return navigationService.GoToLanding();
        }

        public override async Task InitAsync()
        {
            var orderModel = await frontOfficeRestaurantService.GetOrder(Parameter.OrderId);
            if (orderModel != null)
            {
                Order = orderModel.ModelToVm();
                orderStatusTrackingService.StartOrderTracking(orderModel);
                orderStatusTrackingService.OrderStatusChanged += OrderStatusTrackingService_OrderStatusChanged;
            }
   
          
        }

        private void OrderStatusTrackingService_OrderStatusChanged(object sender, OrderStatusChangedEventArgs args)
        {
            if(Order.OrderStatus!= AppCore.Models.OrderStatus.Done && args.UpToDateOrder.OrderStatus== AppCore.Models.OrderStatus.Done)
            {
                vibrationService.Vibrate(2);
                
            }
            Order.OrderStatus = args.UpToDateOrder.OrderStatus; 
        }

        public override Task CleanUp()
        {
            orderStatusTrackingService.StopOrderTracking();
            orderStatusTrackingService.OrderStatusChanged -= OrderStatusTrackingService_OrderStatusChanged;
            return Task.CompletedTask;
        }
    }

    public class WaitingForOrderParams
    {
        public  Guid OrderId { get; set; }
    }


}
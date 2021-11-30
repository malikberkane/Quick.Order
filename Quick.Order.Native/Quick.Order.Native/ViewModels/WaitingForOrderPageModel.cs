using Quick.Order.AppCore.BusinessOperations;
using Quick.Order.AppCore.Contracts;
using Quick.Order.Native.Services;
using Quick.Order.Native.ViewModels.Base;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Quick.Order.Native.ViewModels
{
    public class WaitingForOrderPageModel : ExtendedPageModelBase<WaitingForOrderParams> { 
 

        public ICommand  DismissOrderTrackingCommand { get; set; } 
        public OrderVm Order { get; set; }
        public WaitingForOrderPageModel(OrderStatusTrackingService orderStatusTrackingService, FrontOfficeRestaurantService frontOfficeRestaurantService, INavigationService navigationService, IVibrationService vibrationService)
        {
            
            DismissOrderTrackingCommand = CreateAsyncCommand(GoToLanding);
        }

        private Task GoToLanding()
        {
            return NavigationService.SignIn.GoToLanding();
        }

        public override async Task InitAsync()
        {
            var orderModel = await ServicesAggregate.Repositories.Orders.GetById(Parameter.OrderId);
            if (orderModel != null)
            {
                Order = orderModel.ModelToVm();
                ServicesAggregate.Business.OrdersStatusTracking.StartOrderTracking(orderModel);
                ServicesAggregate.Business.OrdersStatusTracking.OrderStatusChanged += OrderStatusTrackingService_OrderStatusChanged;
            }
   
          
        }

        private void OrderStatusTrackingService_OrderStatusChanged(object sender, OrderStatusChangedEventArgs args)
        {
            try
            {
                if (Order.OrderStatus != AppCore.Models.OrderStatus.Done && args.UpToDateOrder.OrderStatus == AppCore.Models.OrderStatus.Done)
                {
                    ServicesAggregate.Plugin.Vibration.Vibrate(2);

                }
                Order.OrderStatus = args.UpToDateOrder.OrderStatus;
            }
            catch (Exception ex)
            {
                OnExceptionCaught(ex);

            }
        }

        public override Task CleanUp()
        {
            ServicesAggregate.Business.OrdersStatusTracking.StopOrderTracking();
            ServicesAggregate.Business.OrdersStatusTracking.OrderStatusChanged -= OrderStatusTrackingService_OrderStatusChanged;
            return Task.CompletedTask;
        }
    }
    public class WaitingForOrderParams
    {
        public  Guid OrderId { get; set; }
    }


}
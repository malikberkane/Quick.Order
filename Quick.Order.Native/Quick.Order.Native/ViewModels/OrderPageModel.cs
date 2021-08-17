using MalikBerkane.MvvmToolkit;
using Quick.Order.AppCore.BusinessOperations;
using Quick.Order.Native.Services;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Quick.Order.Native.ViewModels
{
    public class OrderPageModel: PageModelBase<AppCore.Models.Order>
    {
        private readonly INavigationService navigationService;
        private readonly BackOfficeRestaurantService backOfficeRestaurantService;
        private readonly PageModelMessagingService messagingService;

        public OrderVm Order { get; private set; }
        public ICommand EditOrderStatusCommand { get; }

        public ICommand DeleteOrderCommand { get; }

        public OrderPageModel(INavigationService navigationService ,BackOfficeRestaurantService backOfficeRestaurantService ,PageModelMessagingService messagingService)
        {
            EditOrderStatusCommand = CreateCommand<OrderVm>(EditOrderStatus);
            DeleteOrderCommand = CreateCommand(DeleteOrder);

            this.navigationService = navigationService;
            this.backOfficeRestaurantService = backOfficeRestaurantService;
            this.messagingService = messagingService;
        }




        private async Task EditOrderStatus(OrderVm order)
        {

            var result = await navigationService.GoToEditOrderStatus(order.VmToModel());

            if (result != null && result.WasSuccessful)
            {
                Order.OrderStatus = result.ValidatedStatus;
                messagingService.Send("OrderStatusEdited", result);
            }


        }

        private async Task DeleteOrder()
        {
            
            await  backOfficeRestaurantService.DeleteOrder(Parameter);

            messagingService.Send("OrderStatusEdited", new OrderStatusEditionResult() { WasSuccessful = true, WasDeleted = true , Order=Order.VmToModel()});

           
        }


        protected override void PostParamInitialization()
        {
            Order = Parameter.ModelToVm();
        }

    }
}

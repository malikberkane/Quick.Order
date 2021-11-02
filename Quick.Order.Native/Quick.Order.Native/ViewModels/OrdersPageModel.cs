using MalikBerkane.MvvmToolkit;
using Quick.Order.AppCore.BusinessOperations;
using Quick.Order.Native.Services;
using Quick.Order.Native.ViewModels.Base;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Quick.Order.Native.ViewModels
{
    public class OrdersPageModel: ExtendedPageModelBase<AppCore.Models.Order>
    {
       

        public OrderVm Order { get; private set; }
        public ICommand EditOrderStatusCommand { get; }

        public ICommand DeleteOrderCommand { get; }

        public OrdersPageModel()
        {
            EditOrderStatusCommand = CreateCommand<OrderVm>(EditOrderStatus);
            DeleteOrderCommand = CreateCommand(DeleteOrder);

        }




        private async Task EditOrderStatus(OrderVm order)
        {

            var result = await NavigationService.BackOffice.GoToEditOrderStatus(order.VmToModel());

            if (result != null && result.WasSuccessful)
            {
                Order.OrderStatus = result.ValidatedStatus;
                MessagingService.Send("OrderStatusEdited", result);
            }


        }

        private async Task DeleteOrder()
        {
            
            await  ServicesAggregate.Business.BackOffice.DeleteOrder(Parameter);

            MessagingService.Send("OrderStatusEdited", new OrderStatusEditionResult() { WasSuccessful = true, WasDeleted = true , Order=Order.VmToModel()});
            await NavigationService.Common.GoBack();
           
        }


        protected override void PostParamInitialization()
        {
            Order = Parameter.ModelToVm();
        }

    }
}

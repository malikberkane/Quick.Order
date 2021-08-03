using MalikBerkane.MvvmToolkit;
using Quick.Order.AppCore.BusinessOperations;
using Quick.Order.AppCore.Models;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Quick.Order.Native.ViewModels
{
    public class EditOrderStatusPageModel: ModalPageModelBase<AppCore.Models.Order, OrderStatusEditionResult>
    {
        private readonly BackOfficeRestaurantService backOfficeRestaurantService;

        public ICommand EditOrderStatusCommand { get; set; }
        public ICommand DeleteOrderCommand { get; }

        public EditOrderStatusPageModel(BackOfficeRestaurantService backOfficeRestaurantService)
        {
            this.backOfficeRestaurantService = backOfficeRestaurantService;
            EditOrderStatusCommand = CreateAsyncCommand<OrderStatus>(EditOrderStatus);
            DeleteOrderCommand = CreateCommand(DeleteOrder);
        }

        private async Task DeleteOrder()
        {
            try
            {
                await backOfficeRestaurantService.DeleteOrder(Parameter);
                await SetResult(new OrderStatusEditionResult() { WasSuccessful = true, WasDeleted=true});
            }
            catch (System.Exception ex)
            {

                await SetResult(new OrderStatusEditionResult() { WasSuccessful = false, ErrorMessage = ex.Message });
            }
        }

        private async Task EditOrderStatus(OrderStatus orderStatus)
        {

            try
            {
                await backOfficeRestaurantService.SetOrderStatus(Parameter,orderStatus);
                await SetResult(new OrderStatusEditionResult() { WasSuccessful = true , ValidatedStatus=orderStatus});
            }
            catch (System.Exception ex)
            {

                await SetResult(new OrderStatusEditionResult() { WasSuccessful = false, ErrorMessage = ex.Message });
            }

        }

    }

    public class OrderStatusEditionResult:OperationResult
    {
        public OrderStatus ValidatedStatus { get; set; }

        public bool WasDeleted { get; set; }
    }

    

    
}


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

        public EditOrderStatusPageModel(BackOfficeRestaurantService backOfficeRestaurantService)
        {
            this.backOfficeRestaurantService = backOfficeRestaurantService;
            EditOrderStatusCommand = CreateAsyncCommand<OrderStatus>(EditOrderStatus);
        }

       

        private async Task EditOrderStatus(OrderStatus orderStatus)
        {

            try
            {
                await backOfficeRestaurantService.SetOrderStatus(Parameter,orderStatus);
                await SetResult(new OrderStatusEditionResult() { WasSuccessful = true , Order= Parameter, ValidatedStatus=orderStatus});
            }
            catch (System.Exception ex)
            {

                await SetResult(new OrderStatusEditionResult() { WasSuccessful = false, ErrorMessage = ex.Message });
            }

        }

    }

    public class OrderStatusEditionResult : OperationResult
    {
        public AppCore.Models.Order Order {get;set;}
        public OrderStatus ValidatedStatus { get; set; }

        public bool WasDeleted { get; set; }
    }

    

    
}


using MalikBerkane.MvvmToolkit;
using Quick.Order.AppCore.BusinessOperations;
using Quick.Order.AppCore.Models;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Quick.Order.Native.ViewModels
{
    public class PlaceOrderPageModel: ModalPageModelBase<PlaceOrderNavigationParams, OrderValidationResult>
    {
        private readonly FrontOfficeRestaurantService frontOfficeRestaurantService;

        public AppCore.Models.Order Order { get; set; }
        public ICommand PlaceOrderCommand { get; set; }
        public Restaurant Restaurant { get; set; }

        public PlaceOrderPageModel(FrontOfficeRestaurantService frontOfficeRestaurantService)
        {
            this.frontOfficeRestaurantService = frontOfficeRestaurantService;
            PlaceOrderCommand = CreateAsyncCommand(PlaceOrder);

        }

        private async Task PlaceOrder()
        {

            try
            {
                if (Order.IsValid())
                {
                    await frontOfficeRestaurantService.PlaceOrder(Order);
                    await SetResult(new OrderValidationResult() { WasSuccessful = true, Order = Order });
                }
                else
                {
                    await SetResult(new OrderValidationResult() { WasSuccessful = false, ErrorMessage = "Order not valid: missing client name or basket" });

                }

            }
            catch (System.Exception ex)
            {

                await SetResult(new OrderValidationResult() { WasSuccessful = false, ErrorMessage=ex.Message });
            }

        }

        public override Task InitAsync()
        {
            Order = Parameter.Order;
            Restaurant = Parameter.Restaurant;
            return Task.CompletedTask;
        }
    }

    public class OrderValidationResult:OperationResult
    {
        public AppCore.Models.Order Order { get; set; } 
    }


    public class PlaceOrderNavigationParams
    {
        public AppCore.Models.Order Order { get; set; }
        public Restaurant Restaurant { get; set; }

    }
}   


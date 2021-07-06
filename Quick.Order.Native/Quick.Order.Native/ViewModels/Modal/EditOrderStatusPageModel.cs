using MalikBerkane.MvvmToolkit;
using Quick.Order.AppCore.BusinessOperations;
using Quick.Order.AppCore.Models;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Quick.Order.Native.ViewModels
{
    public class EditOrderStatusPageModel: ModalPageModelBase<AppCore.Models.Order, OperationResult>
    {
        private readonly BackOfficeRestaurantService backOfficeRestaurantService;

        public ICommand EditOrderStatusCommand { get; set; }
        public EditOrderStatusPageModel(BackOfficeRestaurantService backOfficeRestaurantService)
        {
            this.backOfficeRestaurantService = backOfficeRestaurantService;
            EditOrderStatusCommand = new AsyncCommand<OrderStatus>(async(o)=>await EnsurePageModelIsInLoadingState(async()=>await EditOrderStatus(o)));
        }

        private async Task EditOrderStatus(OrderStatus orderStatus)
        {

            try
            {
                await backOfficeRestaurantService.SetOrderStatus(Parameter,orderStatus);
                await SetResult(new OperationResult() { WasSuccessful = true });
            }
            catch (System.Exception ex)
            {

                await SetResult(new OperationResult() { WasSuccessful = false, ErrorMessage = ex.Message });
            }

        }

    }

    

    
}


using Quick.Order.AppCore.Models;
using Quick.Order.Native.ViewModels;
using Quick.Order.Native.ViewModels.Modal;
using System;
using System.Threading.Tasks;

namespace Quick.Order.Native.Services
{
    public interface ITakeOrderNavigation
    {
        Task GoToMenu(Restaurant restaurant);
        Task<BasketItem> GoToAddItemToBasket(Dish dish);
        Task<EditItemInBasketModalResult> GoToEditBasketItem(BasketItem dish);

        Task<OrderValidationResult> GoToPlaceOrder(AppCore.Models.Order order);


        Task GoToWaitingForOrderContext(Guid guid);
        Task GoToDiscover();

        Task GoToQrCodeScanning();
        Task<string> GoToQrCodeScanningModal();
    }
}
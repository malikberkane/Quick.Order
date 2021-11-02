using Quick.Order.AppCore.Models;
using Quick.Order.Native.Popups;
using Quick.Order.Native.ViewModels;
using Quick.Order.Native.ViewModels.Modal;
using Quick.Order.Native.Views;
using System;
using System.Threading.Tasks;

namespace Quick.Order.Native.Services
{
    public class OrderNavigationService : BaseNavigationService, ITakeOrderNavigation
    {
        public Task GoToMenu(Restaurant restaurant)
        {
            return viewModelNavigationService.PushPage<MenuPage, MenuPageModel>(restaurant);

        }

        public Task<BasketItem> GoToAddItemToBasket(Dish dish)
        {
            return viewModelNavigationService.PushModal<AddItemToBasketPopup, AddItemToBasketPageModel, BasketItem>(dish);
        }


        


        public Task<EditItemInBasketModalResult> GoToEditBasketItem(BasketItem basketItem)
        {
            return viewModelNavigationService.PushModal<EditItemInBasketPopup, EditItemInBasketPageModel, EditItemInBasketModalResult>(basketItem);
        }

        public Task<OrderValidationResult> GoToPlaceOrder(AppCore.Models.Order order)
        {
            return viewModelNavigationService.PushModal<PlaceOrderPopup, PlaceOrderPageModel, OrderValidationResult>(order);
        }


        public Task GoToWaitingForOrderContext(Guid id)
        {

            viewModelNavigationService.CreateNavigationRoot<WaitingForOrderPage, WaitingForOrderPageModel>(new WaitingForOrderParams { OrderId = id });

            return Task.CompletedTask;

        }

        public Task GoToQrCodeScanning()
        {
            GoogleVisionBarCodeScanner.Methods.AskForRequiredPermission();

            return viewModelNavigationService.PushPage<QrCodeScanPage, QrCodeScanPageModel>();


        }

        public async Task<string> GoToQrCodeScanningModal()
        {
            await GoogleVisionBarCodeScanner.Methods.AskForRequiredPermission();

            return await viewModelNavigationService.PushModal<QrCodeModalScanPopup, QrCodeModalScanPageModel, string>();


        }



        public Task GoToDiscover()
        {
            return viewModelNavigationService.PushPage<DiscoverPage, DiscoverPageModel>();
        }

    }
}

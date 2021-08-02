using MalikBerkane.MvvmToolkit;
using Quick.Order.AppCore.Models;
using Quick.Order.Native.Popups;
using Quick.Order.Native.ViewModels;
using Quick.Order.Native.ViewModels.Modal;
using Quick.Order.Native.Views;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Quick.Order.Native.Services
{
    public class NavigationService : INavigationService
    {
        private readonly ViewModelNavigationService viewModelNavigationService;

        public NavigationService(ViewModelNavigationService viewModelNavigationService)
        {
            this.viewModelNavigationService = viewModelNavigationService;
        }
        public Task GoToMainBackOffice()
        {
            viewModelNavigationService.CreateNavigationRoot<BackOfficeHomePage, BackOfficeHomePageModel>();
            return Task.CompletedTask;

        }

        public Task GoToLogin()
        {
            return viewModelNavigationService.PushPage<LoginPage, LoginViewModel>();
        }


        public Task GoToLanding()
        {
            viewModelNavigationService.CreateNavigationRoot<LandingPage, LandingViewModel>();
            return Task.CompletedTask;

        }

        public Task GoToMenuEdition(Restaurant restaurant)
        {
            return viewModelNavigationService.PushPage<MenuEditionPage, MenuEditionPageModel>(restaurant);
        }

        public Task GoToMenu(Restaurant restaurant)
        {
            viewModelNavigationService.CreateNavigationRoot<MenuPage, MenuPageModel>(restaurant);
            return Task.CompletedTask;

        }

        public Task<BasketItem> GoToAddItemToBasket(Dish dish)
        {
            return viewModelNavigationService.PushModal<AddItemToBasketPopup, AddItemToBasketPageModel, BasketItem>(dish);
        }

        public Task GoToRestaurantEdition(Restaurant restaurant = null)
        {
            return viewModelNavigationService.PushPage<NewRestaurantPage, NewRestaurantPageModel>(restaurant);
        }

        public Task GoBack()
        {
            return Application.Current.MainPage.Navigation.PopAsync();
        }

        public Task GoToAddDish(Restaurant restaurant, DishSection section)
        {
            return viewModelNavigationService.PushPage<AddDishPage, AddDishPageModel>(new AddDishParams { Restaurant = restaurant, Section = section });
        }

        public Task<OperationResult> GoToAddDishSection(EditDishSectionParams editDishSectionParams)
        {
            return viewModelNavigationService.PushModal<AddDishSectionPopup, AddDishSectionPageModel, OperationResult>(editDishSectionParams);
        }

        public Task<RestaurantIdentity> GoToEditRestaurantInfos(RestaurantIdentity restaurant)
        {
            return viewModelNavigationService.PushModal<EditRestaurantInfosPopup, EditRestaurantInfosPageModel, RestaurantIdentity>(restaurant);
        }

        public Task GoToEditDish(EditDishParams editDishParams)
        {
            return viewModelNavigationService.PushPage<EditDishPage, EditDishPageModel>(editDishParams);
        }

        public Task<EditItemInBasketModalResult> GoToEditBasketItem(BasketItem basketItem)
        {
            return viewModelNavigationService.PushModal<EditItemInBasketPopup, EditItemInBasketPageModel, EditItemInBasketModalResult>(basketItem);
        }

        public Task<OrderValidationResult> GoToPlaceOrder(AppCore.Models.Order order)
        {
            return viewModelNavigationService.PushModal<PlaceOrderPopup, PlaceOrderPageModel, OrderValidationResult>(order);
        }

        public Task<OrderStatusEditionResult> GoToEditOrderStatus(AppCore.Models.Order order)
        {
            return viewModelNavigationService.PushModal<EditOrderStatusPopup, EditOrderStatusPageModel, OrderStatusEditionResult>(order);
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

        public Task<string> GoToQrCodeScanningModal()
        {
            GoogleVisionBarCodeScanner.Methods.AskForRequiredPermission();

            return viewModelNavigationService.PushModal<QrCodeModalScanPopup, QrCodeModalScanPageModel,string>();


        }

    }
}

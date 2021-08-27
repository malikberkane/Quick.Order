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

        public Task<bool> PromptForConfirmation(string title, string message, string confirm, string cancel = null)
        {
            return Application.Current.MainPage.DisplayAlert(title, message, confirm, cancel);
        }
        public Task GoToLanding(string scannedCode = null)
        {
            viewModelNavigationService.CreateNavigationRoot<LandingPage, LandingViewModel>(scannedCode);
            return Task.CompletedTask;

        }

        public Task GoToMenuEdition(Restaurant restaurant)
        {
            return Task.CompletedTask ;
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

        public Task<Restaurant> GoToRestaurantEdition(Restaurant restaurant = null)
        {
            return viewModelNavigationService.PushModal<NewRestaurantPopup, NewRestaurantPageModel, Restaurant>(restaurant);
        }

        public Task GoBack()
        {
            return Application.Current.MainPage.Navigation.PopAsync();
        }

        public Task<DishEditionResult> GoToAddDish(AddDishParams addDishParams)
        {
            return viewModelNavigationService.PushModal<AddDishPopup, AddDishPageModel, DishEditionResult>(addDishParams);
        }

        public Task<DishSectionEditionOperationResult> GoToAddOrEditDishSection(EditDishSectionParams editDishSectionParams)
        {
            return viewModelNavigationService.PushModal<AddDishSectionPopup, AddDishSectionPageModel, DishSectionEditionOperationResult>(editDishSectionParams);
        }

        public Task<RestaurantIdentity> GoToEditRestaurantInfos(RestaurantIdentity restaurant)
        {
            return viewModelNavigationService.PushModal<EditRestaurantInfosPopup, EditRestaurantInfosPageModel, RestaurantIdentity>(restaurant);
        }

        public Task<DishEditionResult> GoToEditDish(EditDishParams editDishParams)
        {
            return viewModelNavigationService.PushModal<EditDishPopup, EditDishPageModel, DishEditionResult>(editDishParams);
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

        public async Task<string> GoToQrCodeScanningModal()
        {
            await GoogleVisionBarCodeScanner.Methods.AskForRequiredPermission();

            return await viewModelNavigationService.PushModal<QrCodeModalScanPopup, QrCodeModalScanPageModel,string>();


        }

        public Task GoToOrderDetails(AppCore.Models.Order order)
        {
            return viewModelNavigationService.PushPage<OrderPage, OrderPageModel>(order);
        }

        public Task GoToQrGeneration(Restaurant restaurant)
        {
            return viewModelNavigationService.PushPage<QrCodeGenerationPage, QrCodeGenerationPageModel>(restaurant);

        }

        public Task GoToDiscover()
        {
            return viewModelNavigationService.PushPage<DiscoverPage, DiscoverPageModel>();
        }
    }
}

using Quick.Order.AppCore.Models;
using Quick.Order.Native.ViewModels;
using Quick.Order.Native.ViewModels.Modal;
using System;
using System.Threading.Tasks;

namespace Quick.Order.Native.Services
{
    public interface INavigationService
    {

        Task GoToLanding(string scannedCode=null);
        Task GoToLogin();
        Task GoToMainBackOffice();
        Task GoToDiscover();

        Task GoToMenuEdition(Restaurant restaurant);

        Task GoToMenu(Restaurant restaurant);
        Task<BasketItem> GoToAddItemToBasket(Dish dish);
        Task<EditItemInBasketModalResult> GoToEditBasketItem(BasketItem dish);


        Task GoBack();
        Task<Restaurant> GoToRestaurantEdition(Restaurant restaurant = null);

        Task<DishEditionResult> GoToAddDish(AddDishParams addDishParams);
        Task<DishEditionResult> GoToEditDish(EditDishParams editDishParams);

        Task<OrderStatusEditionResult> GoToEditOrderStatus(AppCore.Models.Order order);

        Task<DishSectionEditionOperationResult> GoToAddOrEditDishSection(EditDishSectionParams editDishSectionParams);

        Task<OrderValidationResult> GoToPlaceOrder(AppCore.Models.Order order);
        Task GoToWaitingForOrderContext(Guid guid);
        Task GoToOrderDetails(AppCore.Models.Order Order);

        Task<RestaurantIdentity> GoToEditRestaurantInfos(RestaurantIdentity restaurant);
        Task GoToQrCodeScanning();
        Task<string> GoToQrCodeScanningModal();
        Task GoToQrGeneration(Restaurant restaurant);
        Task<bool> PromptForConfirmation(string title, string message, string confirm, string cancel = null);
    }


    public interface IFileService
    {

    }
}
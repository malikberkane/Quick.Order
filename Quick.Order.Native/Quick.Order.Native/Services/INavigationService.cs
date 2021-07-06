using Quick.Order.AppCore.Models;
using Quick.Order.Native.ViewModels;
using Quick.Order.Native.ViewModels.Modal;
using System;
using System.Threading.Tasks;

namespace Quick.Order.Native.Services
{
    public interface INavigationService
    {
        Task GoToLanding();
        Task GoToLogin();
        Task GoToMainBackOffice();

        Task GoToMenuEdition(Restaurant restaurant);

        Task GoToMenu(Restaurant restaurant);
        Task<BasketItem> GoToAddItemToBasket(Dish dish);
        Task<EditItemInBasketModalResult> GoToEditBasketItem(BasketItem dish);


        Task GoBack();
        Task GoToRestaurantEdition(Restaurant restaurant = null);

        Task GoToAddDish(Restaurant restaurant, DishSection section);
        Task GoToEditDish(EditDishParams editDishParams);

        Task<OperationResult> GoToEditOrderStatus(AppCore.Models.Order order);

        Task<OperationResult> GoToAddDishSection(Restaurant restaurant);

        Task<OrderValidationResult> GoToPlaceOrder(AppCore.Models.Order order);
        Task GoToWaitingForOrderContext(Guid guid);

        Task<RestaurantIdentity> GoToEditRestaurantInfos(RestaurantIdentity restaurant);

    }
}
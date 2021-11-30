using Quick.Order.AppCore.Models;
using Quick.Order.Native.ViewModels;
using Quick.Order.Native.ViewModels.Modal;
using System.Threading.Tasks;

namespace Quick.Order.Native.Services
{
    public interface IBackOfficeNavigation
    {
        Task GoToMainBackOffice();

        Task<Restaurant> GoToRestaurantEdition(Restaurant restaurant = null);

        Task<DishEditionResult> GoToAddDish(AddDishParams addDishParams);
        Task<DishEditionResult> GoToEditDish(EditDishParams editDishParams);

        Task<OrderStatusEditionResult> GoToEditOrderStatus(AppCore.Models.Order order);

        Task<DishSectionEditionOperationResult> GoToAddOrEditDishSection(EditDishSectionParams editDishSectionParams);

        Task GoToQrGeneration(Restaurant restaurant);

        Task GoToOrderDetails(AppCore.Models.Order Order);

        Task<RestaurantIdentity> GoToEditRestaurantInfos(RestaurantIdentity restaurant);

        Task<Currency> GoToCurrencyChoice();
        Task GoToCultureChoice();
    }
}
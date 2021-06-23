using Quick.Order.AppCore.Models;
using System.Threading.Tasks;

namespace Quick.Order.Native.Services
{
    public interface INavigationService
    {
        Task GoToLanding();
        Task GoToLogin();
        Task GoToMainBackOffice();

        Task GoToMenuEdition(Restaurant restaurant);

        Task GoBack();
        Task GoToRestaurantEdition(Restaurant restaurant = null);

        Task GoToAddDish(Restaurant restaurant, DishSection section);

        Task GoToAddDishSection(Restaurant restaurant);


    }
}
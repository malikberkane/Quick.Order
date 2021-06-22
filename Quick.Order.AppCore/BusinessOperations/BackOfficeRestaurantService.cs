using Quick.Order.AppCore.Authentication.Contracts;
using Quick.Order.AppCore.Contracts.Repositories;
using Quick.Order.AppCore.Exceptions;
using Quick.Order.AppCore.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Quick.Order.AppCore.BusinessOperations
{
    public class BackOfficeRestaurantService
    {
        private readonly IRestaurantRepository restaurantRepository;
        private readonly MenuService menuService;
        private readonly IAuthenticationService authenticationService;

        public BackOfficeRestaurantService(IRestaurantRepository restaurantRepository, MenuService menuService, IAuthenticationService authenticationService)
        {
            this.restaurantRepository = restaurantRepository;
            this.menuService = menuService;
            this.authenticationService = authenticationService;
        }
        public async Task<Restaurant> AddRestaurant(Restaurant restaurant)
        {
            if (restaurant.Administrator == null && authenticationService.LoggedUser!=null)
            {
                restaurant.SetAdministator(authenticationService.LoggedUser.RestaurantAdmin);
            }
            else
            {
                throw new SettingAdminForNewRestaurantException();
            }
            var createdRestaurant= await restaurantRepository.Add(restaurant);
            var menu = new Menu() { Restaurant = createdRestaurant };
            await menuService.AddMenu(menu);
            return createdRestaurant;
        }

        public Task<bool> DeleteRestaurant(Restaurant restaurant)
        {
            return restaurantRepository.Delete(restaurant);
        }

        public Task<bool> UpdateRestaurant(Restaurant restaurant)
        {
            return restaurantRepository.Delete(restaurant);
        }

        public Task<IEnumerable<Restaurant>> GetAllRestaurantsForAccount()
        {
            if (authenticationService.LoggedUser == null)
            {
                throw new UserNotLoggedException();
            }
            return restaurantRepository.Get(r => r.Administrator.Equals(authenticationService.LoggedUser.RestaurantAdmin));
        }
    }
}

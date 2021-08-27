using Quick.Order.AppCore.BusinessOperations;
using Quick.Order.AppCore.Models;
using Quick.Order.Native.Services;
using Quick.Order.Native.ViewModels.Base;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Quick.Order.Native.ViewModels
{
    public class DiscoverPageModel : ExtendedPageModelBase
    {
        public ICommand SelectRestaurantCommand { get; }

        private readonly FrontOfficeRestaurantService frontOfficeRestaurantService;
        private readonly INavigationService navigationService;

        public List<Restaurant> Restaurants { get; set; }


        public DiscoverPageModel(FrontOfficeRestaurantService frontOfficeRestaurantService, INavigationService navigationService)
        {
            SelectRestaurantCommand = CreateAsyncCommand<Restaurant>(SelectRestaurant);
            this.frontOfficeRestaurantService = frontOfficeRestaurantService;
            this.navigationService = navigationService;
        }

        private Task SelectRestaurant(Restaurant restaurant)
        {
            return navigationService.GoToMenu(restaurant);
        }

        public override  async Task InitAsync()
        {
            var restaurants= await frontOfficeRestaurantService.GetAllRestaurants();

            if (restaurants != null)
            {
                Restaurants = new List<Restaurant>(restaurants);
            }
        }
    }
}

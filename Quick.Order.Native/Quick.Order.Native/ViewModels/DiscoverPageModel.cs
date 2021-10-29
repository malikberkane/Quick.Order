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


        public List<Restaurant> Restaurants { get; set; }


        public DiscoverPageModel()
        {
            SelectRestaurantCommand = CreateAsyncCommand<Restaurant>(SelectRestaurant);
            
        }

        private Task SelectRestaurant(Restaurant restaurant)
        {
            return NavigationService.GoToMenu(restaurant);
        }

        public override  async Task InitAsync()
        {
            var restaurants= await ServicesAggregate.Repositories.Restaurants.Get();

            if (restaurants != null)
            {
                Restaurants = new List<Restaurant>(restaurants);
            }
        }
    }
}

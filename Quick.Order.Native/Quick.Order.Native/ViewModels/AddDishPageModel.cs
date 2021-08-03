using MalikBerkane.MvvmToolkit;
using Quick.Order.AppCore.BusinessOperations;
using Quick.Order.AppCore.Models;
using Quick.Order.Native.Services;
using Quick.Order.Native.ViewModels.Base;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Quick.Order.Native.ViewModels
{
    public class AddDishPageModel : ExtendedPageModelBase<AddDishParams>
    {
        private readonly BackOfficeRestaurantService backOfficeRestaurantService;
        private readonly INavigationService navigationService;

        public ICommand AddDishCommand { get; set; }

        public string DishName { get; set; }

        public Restaurant CurrentRestaurant { get; set; }

        public DishSection DishSection { get; set; }
        public AddDishPageModel(BackOfficeRestaurantService backOfficeRestaurantService, PageModelMessagingService messagingService, INavigationService navigationService)
        {
            AddDishCommand = new Command(AddDish);
            this.backOfficeRestaurantService = backOfficeRestaurantService;
            this.navigationService = navigationService;
        }

        private async void AddDish(object obj)
        {
            DishSection.AddDish(new Dish { Name = DishName });

            await backOfficeRestaurantService.UpdateRestaurant(CurrentRestaurant);

            await navigationService.GoBack();

        }


        protected override void PostParamInitialization()
        {
            CurrentRestaurant = Parameter.Restaurant;
            DishSection = Parameter.Section;
        }
    }

    public class AddDishParams
    {
        public Restaurant Restaurant { get; set; }

        public DishSection Section { get; set; }
    }


}


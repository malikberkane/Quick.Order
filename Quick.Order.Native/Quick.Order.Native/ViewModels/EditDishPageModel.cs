using MalikBerkane.MvvmToolkit;
using Quick.Order.AppCore.BusinessOperations;
using Quick.Order.AppCore.Models;
using Quick.Order.Native.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Quick.Order.Native.ViewModels
{
    public class EditDishPageModel: PageModelBase<EditDishParams>
    {
        private readonly BackOfficeRestaurantService backOfficeRestaurantService;
        private readonly INavigationService navigationService;

        public ICommand ValidateCommand { get; set; }

        public ICommand DeleteDishCommand { get; set; }


        public Restaurant CurrentRestaurant { get; set; }

        public Dish CurrentDish { get; set; }

        public Dish EditedDish { get; set; }

        public DishSection CurrentDishSection { get; set; }

        public EditDishPageModel(BackOfficeRestaurantService backOfficeRestaurantService, PageModelMessagingService messagingService, INavigationService navigationService)
        {
            ValidateCommand = new AsyncCommand(Validate);
            DeleteDishCommand = new AsyncCommand(DeleteDish);
            this.backOfficeRestaurantService = backOfficeRestaurantService;
            this.navigationService = navigationService;
        }

        private async Task DeleteDish()
        {
            CurrentDishSection.Remove(CurrentDish);
            await EnsurePageModelIsInLoadingState(async () =>
            {
                await backOfficeRestaurantService.UpdateRestaurant(CurrentRestaurant);

            }); await navigationService.GoBack();
        }

        private async Task Validate()
        {
            if (EditedDish.IsValid())
            {
                CurrentDishSection.UpdateDish(CurrentDish, EditedDish);
                await backOfficeRestaurantService.UpdateRestaurant(CurrentRestaurant);

            }

            await navigationService.GoBack();

        }

        public override Task InitAsync()
        {
            CurrentDish = Parameter.Dish;
            CurrentRestaurant = Parameter.Restaurant;
            CurrentDishSection = Parameter.Restaurant.Menu.GetDishSection(Parameter.Dish);
            EditedDish = Parameter.Dish.Clone();
            return base.InitAsync();
        }
    }

    public class EditDishParams
    {
        public Restaurant Restaurant { get; set; }

        public Dish Dish { get; set; }


    }

}


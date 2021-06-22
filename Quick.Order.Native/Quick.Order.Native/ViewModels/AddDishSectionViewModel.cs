using Quick.Order.AppCore.BusinessOperations;
using Quick.Order.AppCore.Models;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace Quick.Order.Native.ViewModels
{
    public class AddDishSectionViewModel : BaseViewModel
    {
        private readonly BackOfficeRestaurantService backOfficeRestaurantService;

        public ICommand AddDishSectionCommand { get; set; }

        public string DishSectionName { get; set; }

        public Restaurant CurrentRestaurant { get; set; }
        public AddDishSectionViewModel(BackOfficeRestaurantService backOfficeRestaurantService)
        {
            AddDishSectionCommand = new Command(AddDishSection);
            this.backOfficeRestaurantService = backOfficeRestaurantService;
        }

        private async void AddDishSection(object obj)
        {
            if (CurrentRestaurant.Menu.Dishes == null)
            {
                CurrentRestaurant.Menu.Dishes = new List<DishSection>();
            }
            CurrentRestaurant.Menu.Dishes.Add(new DishSection { Name = DishSectionName });

            await backOfficeRestaurantService.UpdateRestaurant(CurrentRestaurant);
        }
    }


}


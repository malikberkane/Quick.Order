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
           
            CurrentRestaurant.AddDishSectionToMenu(new DishSection { Name = DishSectionName });

            await backOfficeRestaurantService.UpdateRestaurant(CurrentRestaurant);

            await Shell.Current.Navigation.PopModalAsync();

        }
    }


}


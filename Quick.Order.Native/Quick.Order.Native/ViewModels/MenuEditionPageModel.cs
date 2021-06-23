using MalikBerkane.MvvmToolkit;
using Quick.Order.AppCore.BusinessOperations;
using Quick.Order.AppCore.Models;
using Quick.Order.Native.Models;
using Quick.Order.Native.Services;
using Quick.Order.Native.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Quick.Order.Native.ViewModels
{
    public class MenuEditionPageModel : PageModelBase<Restaurant>
    {
      
        private readonly FrontOfficeRestaurantService restaurantService;
        private readonly INavigationService navigationService;

        public ICommand AddDishCommand { get; set; }
        public ICommand AddDishSectionCommand { get; set; }

        public string Id { get; set; }

        public ObservableCollection<DishSectionGroupedModel> MenuGroupedBySection { get; set; } = new ObservableCollection<DishSectionGroupedModel>();
        public MenuEditionPageModel(FrontOfficeRestaurantService restaurantService, INavigationService navigationService)
        {
            AddDishCommand = new Command<string>(AddDish);
            AddDishSectionCommand = new Command(AddDishSection);
            this.restaurantService = restaurantService;
            this.navigationService = navigationService;
        }

        private async void AddDishSection()
        {
            await navigationService.GoToAddDishSection(CurrentRestaurant);

        }

        private async void AddDish(string sectionName)
        {

            var section = CurrentRestaurant.Menu.Sections.SingleOrDefault(n => n.Name == sectionName);

            await navigationService.GoToAddDish(CurrentRestaurant, section);
       
        }


        public Restaurant CurrentRestaurant { get; set; }
        public string Text { get; set; }

        public string Description { get; set; }



        public override Task InitAsync()
        {
          
            return LoadMenu();
        }

        protected override Task Refresh()
        {
            return LoadMenu();
        }

        private async Task LoadMenu()
        {
            var item = await restaurantService.GetRestaurantById(Parameter.Id);
            CurrentRestaurant = item;

            if (MenuGroupedBySection != null)
            {
                MenuGroupedBySection.Clear();
            }
            
            foreach (var section in item.Menu.Sections)
            {
                var newSection = new DishSectionGroupedModel { SectionName = section.Name };


                foreach (var dish in section.Dishes)
                {
                    newSection.Add(dish);
                }

                MenuGroupedBySection.Add(newSection);
            }
            Text = item.Name;
            Description = item.Adresse;
        }
    }


}


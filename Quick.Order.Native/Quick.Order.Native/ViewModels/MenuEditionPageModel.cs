using MalikBerkane.MvvmToolkit;
using Quick.Order.AppCore.BusinessOperations;
using Quick.Order.AppCore.Exceptions;
using Quick.Order.AppCore.Models;
using Quick.Order.Native.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Quick.Order.Native.ViewModels
{
    public class MenuEditionPageModel : PageModelBase<Restaurant>
    {
      
        private readonly FrontOfficeRestaurantService restaurantService;
        private readonly BackOfficeRestaurantService backOfficeService;
        private readonly INavigationService navigationService;

        public ICommand AddDishCommand { get; set; }
        public ICommand AddDishSectionCommand { get; set; }

        public ICommand DeleteRestaurantCommand { get; set; }
        public ICommand EditSectionCommand { get; }
        public ICommand GoToEditRestaurantInfosCommand { get; set; }

        public ICommand GoToEditDishCommand { get; set; }

        public string Id { get; set; }

        public ObservableCollection<DishSectionGroupedModel> MenuGroupedBySection { get; set; } = new ObservableCollection<DishSectionGroupedModel>();
        public MenuEditionPageModel(FrontOfficeRestaurantService restaurantService, BackOfficeRestaurantService backOfficeService, INavigationService navigationService)
        {
            AddDishCommand = new AsyncCommand<string>(AddDish);
            AddDishSectionCommand = new AsyncCommand(AddDishSection);
            DeleteRestaurantCommand = new AsyncCommand(DeleteCurrentRestaurant);
            EditSectionCommand = CreateAsyncCommand<string>(EditSection);
            GoToEditRestaurantInfosCommand = new AsyncCommand(GoToEditRestaurantInfos);
            GoToEditDishCommand = new AsyncCommand<Dish>(EditDish);
            this.restaurantService = restaurantService;
            this.backOfficeService = backOfficeService;
            this.navigationService = navigationService;
        }

        private async Task EditSection(string sectionName)
        {
            var result = await navigationService.GoToAddDishSection(new EditDishSectionParams() { Restaurant = CurrentRestaurant, DishSectionToEdit= CurrentRestaurant.Menu.Sections.SingleOrDefault(s=>s.Name==sectionName) });

            if (result!=null && result.WasSuccessful)
            {
                await LoadMenu();
            }
        }

        private Task EditDish(Dish dishToEdit)
        {
            return navigationService.GoToEditDish(new EditDishParams { Dish = dishToEdit, Restaurant = CurrentRestaurant });
        }

        private async Task GoToEditRestaurantInfos()
        {
            var restaurantEdited = await navigationService.GoToEditRestaurantInfos(new Modal.RestaurantIdentity { Adresse = CurrentRestaurant.Adresse,Name=CurrentRestaurant.Name }); 

            if (restaurantEdited != null && restaurantEdited.IsValid())
            {
                CurrentRestaurant.EditIdentity(restaurantEdited.Name, restaurantEdited.Adresse);


                await EnsurePageModelIsInLoadingState(async () =>
                {
                    await backOfficeService.UpdateRestaurant(CurrentRestaurant);

                    await LoadMenu();
                });
            }
        }

        private async Task AddDishSection()
        {
            var result=await navigationService.GoToAddDishSection(new EditDishSectionParams() {  Restaurant= CurrentRestaurant});

            if (result.WasSuccessful)
            {
                await LoadMenu();
            }

        }

        private async Task AddDish(string sectionName)
        {

            var section = CurrentRestaurant.Menu.Sections.SingleOrDefault(n => n.Name == sectionName);

            await navigationService.GoToAddDish(CurrentRestaurant, section);
       
        }

        private async Task DeleteCurrentRestaurant()
        {

            await backOfficeService.DeleteRestaurant(CurrentRestaurant);

            await navigationService.GoBack();
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
            var currentRestaurant = await restaurantService.GetRestaurantById(Parameter.Id);

            if (currentRestaurant == null)
            {
                throw new RestaurantNotFoundException();
            }
            CurrentRestaurant = currentRestaurant;

            if (MenuGroupedBySection != null)
            {
                MenuGroupedBySection.Clear();
            }
            
            foreach (var section in currentRestaurant.Menu.Sections)
            {
                var newSection = new DishSectionGroupedModel { SectionName = section.Name };


                foreach (var dish in section.Dishes)
                {
                    newSection.Add(dish);
                }

                MenuGroupedBySection.Add(newSection);
            }
            Text = currentRestaurant.Name;
            Description = currentRestaurant.Adresse;
        }
    }


}


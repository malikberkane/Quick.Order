using Quick.Order.AppCore.BusinessOperations;
using Quick.Order.AppCore.Models;
using Quick.Order.Native.Models;
using Quick.Order.Native.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Quick.Order.Native.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class ItemDetailViewModel : BaseViewModel
    {
        private string itemId;
        private string text;
        private string description;
        private readonly FrontOfficeRestaurantService restaurantService;

        public ICommand AddDishCommand { get; set; }
        public ICommand AddDishSectionCommand { get; set; }

        public string Id { get; set; }

        public ObservableCollection<DishSectionViewModel> MenuGroupedBySection { get; set; } = new ObservableCollection<DishSectionViewModel>();
        public ItemDetailViewModel(FrontOfficeRestaurantService restaurantService)
        {
            AddDishCommand = new Command(AddDish);
            AddDishSectionCommand = new Command(AddDishSection);
            this.restaurantService = restaurantService;
        }

        private async void AddDishSection()
        {
            var addDishSectionModal = new AddDishSectionPage();

            var bindingContext = FreshMvvm.FreshIOC.Container.Resolve<AddDishSectionViewModel>();
            if (bindingContext != null)
            {
                bindingContext.CurrentRestaurant = CurrentRestaurant;
                addDishSectionModal.BindingContext = bindingContext;
            }
            await Shell.Current.Navigation.PushModalAsync(addDishSectionModal);
        }

        private void AddDish()
        {
            throw new NotImplementedException();
        }


        public Restaurant CurrentRestaurant { get; set; }
        public string Text
        {
            get => text;
            set => SetProperty(ref text, value);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public string ItemId
        {
            get
            {
                return itemId;
            }
            set
            {
                itemId = value;
                LoadItemId(value);
            }
        }

        public async void LoadItemId(string itemId)
        {
            try
            {
                var item = await restaurantService.GetRestaurantById(Guid.Parse(itemId));
                CurrentRestaurant = item;
                foreach (var section in item.Menu.Dishes)
                {
                    var newSection = new DishSectionViewModel { SectionName = section.Name };
                    
                    foreach (var dish in section.Dishes)
                    {
                        newSection.Add(dish);
                    }

                    MenuGroupedBySection.Add(newSection);
                }
                Text = item.Name;
                Description = item.Adresse;


            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }
    }

    public class DishSectionViewModel: ObservableCollection<Dish>, INotifyPropertyChanged
    {
        public string SectionName { get; set; }
    }


}


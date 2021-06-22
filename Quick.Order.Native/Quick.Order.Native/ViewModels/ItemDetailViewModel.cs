using Quick.Order.AppCore.BusinessOperations;
using Quick.Order.AppCore.Models;
using Quick.Order.Native.Models;
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
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class ItemDetailViewModel : BaseViewModel
    {
        private string itemId;
        private string text;
        private string description;
        private readonly MenuService menuService;

        public ICommand AddDishCommand { get; set; }
        public ICommand AddDishSectionCommand { get; set; }

        public string Id { get; set; }

        public ObservableCollection<DishSectionViewModel> MenuGroupedBySection { get; set; } = new ObservableCollection<DishSectionViewModel>();
        public ItemDetailViewModel(MenuService menuService)
        {
            this.menuService = menuService;
            AddDishCommand = new Command(AddDish);
            AddDishSectionCommand = new Command(AddDishSection);

        }

        private async void AddDishSection()
        {
            var addDishSectionModal = new AddDishSectionPage();

            var bindingContext = FreshMvvm.FreshIOC.Container.Resolve<AddDishSectionViewModel>();
            if (bindingContext != null)
            {
                bindingContext.CurrentMenu = CurrentMenu;
                addDishSectionModal.BindingContext = bindingContext;
            }
            await Shell.Current.Navigation.PushModalAsync(addDishSectionModal);
        }

        private void AddDish()
        {
            throw new NotImplementedException();
        }


        public AppCore.Models.Menu CurrentMenu { get; set; }
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
                var item = await menuService.GetMenu(itemId);
                CurrentMenu = item;
                foreach (var section in item.Dishes)
                {
                    var newSection = new DishSectionViewModel { SectionName = section.Name };

                    foreach (var dish in section.Dishes)
                    {
                        newSection.Add(dish);
                    }

                    MenuGroupedBySection.Add(newSection);
                }
                Text = item.Restaurant.Name;
                Description = item.Restaurant.Adresse;


            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }
    }

    public class DishSectionViewModel: ObservableCollection<Dish>
    {
        public string SectionName { get; set; }
    }


}


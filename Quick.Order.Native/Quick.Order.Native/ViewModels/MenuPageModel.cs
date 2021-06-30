using MalikBerkane.MvvmToolkit;
using Quick.Order.AppCore.BusinessOperations;
using Quick.Order.AppCore.Models;
using Quick.Order.Native.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Quick.Order.Native.ViewModels
{
    public class MenuPageModel : PageModelBase<Restaurant>
    {
        private readonly INavigationService navigationService;
        private readonly FrontOfficeRestaurantService frontOfficeRestaurantService;

        public Restaurant Restaurant { get; set; }

        public ICommand PlaceOrderCommand { get; set; }

        public ICommand AddItemToBasketCommand { get; set; }
        public ICommand EditBasketItemCommand { get; set; }


        public MenuPageModel(INavigationService navigationService, FrontOfficeRestaurantService frontOfficeRestaurantService)
        {
            AddItemToBasketCommand = new AsyncCommand<Dish>(AddItemToBasket);
            EditBasketItemCommand = new AsyncCommand<BasketItem>(EditBasketItem);
            PlaceOrderCommand = new AsyncCommand(PlaceOrder);

            this.navigationService = navigationService;
            this.frontOfficeRestaurantService = frontOfficeRestaurantService;
        }

        private async Task EditBasketItem(BasketItem basketItem)
        {
            var result = await navigationService.GoToEditBasketItem(basketItem);

            if (result != null)
            {
                if (result.NeedToBeDeleted)
                {
                    Basket.Remove(basketItem);
                    OnPropertyChanged(nameof(BasketCount));

                }
                else
                {
                    var editedItemIndex = Basket.IndexOf(basketItem);
                    Basket[editedItemIndex] = editedItemIndex != -1 ? result.BasketItem : throw new Exception("Edited index not found");

                }
            }
        }

        public ObservableCollection<DishSectionGroupedModel> MenuGroupedBySection { get; set; } = new ObservableCollection<DishSectionGroupedModel>();

        public ObservableCollection<BasketItem> Basket { get; set; } = new ObservableCollection<BasketItem>();

        public int BasketCount => Basket?.Count ?? default;
        public override Task InitAsync()
        {
            Restaurant = Parameter;
            LoadMenu();

            return Task.CompletedTask;
        }


        private async Task AddItemToBasket(Dish dish)
        {
            var result = await navigationService.GoToAddItemToBasket(dish);

            if (result != null)
            {
                Basket.Add(result);
                OnPropertyChanged(nameof(BasketCount));
            }
        }


        private Task PlaceOrder()
        {
            return frontOfficeRestaurantService.PlaceOrder(Quick.Order.AppCore.Models.Order.CreateNew(Restaurant, Basket));
        }
        private void LoadMenu()
        {
            if (Restaurant?.Menu == null)
            {
                throw new System.Exception("Restaurant or menu is null: impossible to group menu by sections");
            }

            if (MenuGroupedBySection != null)
            {
                MenuGroupedBySection.Clear();
            }


            foreach (var section in Restaurant.Menu.Sections)
            {
                var newSection = new DishSectionGroupedModel { SectionName = section.Name };


                foreach (var dish in section.Dishes)
                {
                    newSection.Add(dish);
                }

                MenuGroupedBySection.Add(newSection);
            }
            
        }

    }



}
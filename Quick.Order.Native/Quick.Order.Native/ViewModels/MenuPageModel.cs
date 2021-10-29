using Quick.Order.AppCore.Contracts;
using Quick.Order.AppCore.Models;
using Quick.Order.Native.Services;
using Quick.Order.Native.ViewModels.Base;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Quick.Order.Native.ViewModels
{
    public partial class MenuPageModel : ExtendedPageModelBase<Restaurant>
    {
      
        public ICommand GoBackCommand { get; set; }


        public Restaurant Restaurant { get; set; }

        public ICommand PlaceOrderCommand { get; set; }

        public ICommand AddItemToBasketCommand { get; set; }
        public ICommand EditBasketItemCommand { get; set; }


        public MenuPageModel(INavigationService navigationService,ILocalHistoryService localHistoryService)
        {
            AddItemToBasketCommand = CreateCommand<Dish>(AddItemToBasket);
            EditBasketItemCommand = CreateCommand<BasketItem>(EditBasketItem);
            PlaceOrderCommand = CreateCommand(PlaceOrder);
            GoBackCommand = CreateCommand(navigationService.LeaveRestaurantMenu);

        }

        private async Task EditBasketItem(BasketItem basketItem)
        {
            var result = await NavigationService.GoToEditBasketItem(basketItem);

            if (result != null)
            {
                if (result.NeedToBeDeleted)
                {
                    Basket.Remove(basketItem);
                    OnPropertyChanged(nameof(BasketCount));
                    OnPropertyChanged(nameof(Basket));

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
       
        protected override void PostParamInitialization()
        {
            Restaurant = Parameter;

            var localOrder = ServicesAggregate.Business.LocalHistory.GetLocalOrder();
            
            LoadMenu();

            if (localOrder != null && localOrder.RestaurantId==Parameter.Id)
            {
                Basket = new ObservableCollection<BasketItem>(localOrder.OrderedItems);
            }
        }

        private async Task AddItemToBasket(Dish dish)
        {
            var basketItemToEdit = Basket.SingleOrDefault(n => n.Dish.Equals(dish));

            if (basketItemToEdit != null)
            {
                await EditBasketItem(basketItemToEdit);
            }
            else
            {
                var result = await NavigationService.GoToAddItemToBasket(dish);

                if (result != null)
                {
                    Basket.Add(result);
                    OnPropertyChanged(nameof(BasketCount));
                    OnPropertyChanged(nameof(Basket));

                }
            }

           
        }

        private async Task PlaceOrder()
        {
            var orderValidationResult= await NavigationService.GoToPlaceOrder(Quick.Order.AppCore.Models.Order.CreateNew(Restaurant, Basket));

            if (orderValidationResult != null)
            {
                if (orderValidationResult.WasSuccessful)
                {
                    if (orderValidationResult.Order == null)
                    {
                        throw new Exception("Error retrieving validated order");
                    }
                    else
                    {
                        await NavigationService.GoToWaitingForOrderContext(orderValidationResult.Order.Id);

                    }
                }
                else
                {
                    HandleError(orderValidationResult.ErrorMessage ?? "Validation unsuccessful");
                }


            }

        }
        private void LoadMenu()
        {
            if (Restaurant?.Menu == null)
            {
                throw new Exception("Restaurant or menu is null: impossible to group menu by sections");
            }

            if (MenuGroupedBySection != null)
            {
                MenuGroupedBySection.Clear();
            }


            foreach (var section in Restaurant.Menu.Sections)
            {
                var newSection = new DishSectionGroupedModel { SectionName = section.Name };


                foreach (var dish in section.GetDishes())
                {
                    newSection.Add(dish);
                }

                MenuGroupedBySection.Add(newSection);
            }
            
        }


        public override void OnDisappearing(object sender, EventArgs e)
        {
            if(Basket!=null && Basket.Any())
            {
                ServicesAggregate.Business.LocalHistory.SaveLocalOrder(AppCore.Models.Order.CreateNew(Restaurant, Basket));

            }
            base.OnDisappearing(sender, e);
        }
    }



}
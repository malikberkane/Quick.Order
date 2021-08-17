using MalikBerkane.MvvmToolkit;
using Quick.Order.AppCore.Authentication.Contracts;
using Quick.Order.AppCore.BusinessOperations;
using Quick.Order.AppCore.Exceptions;
using Quick.Order.AppCore.Models;
using Quick.Order.Native.Services;
using Quick.Order.Native.ViewModels.Base;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Quick.Order.Native.ViewModels
{
    public class BackOfficeHomePageModel : ExtendedPageModelBase
    {
        private readonly BackOfficeRestaurantService backOfficeService;
        private readonly FrontOfficeRestaurantService restaurantService;
        private readonly INavigationService navigationService;
        private readonly PageModelMessagingService messagingService;
        private readonly IAuthenticationService authenticationService;

        public ObservableCollection<Restaurant> Items { get; }

        public ObservableCollection<OrderVm> Orders { get; set; }

        public RestaurantAdmin CurrentLoggedAccount { get; set; }
        public ICommand LogoutCommand { get; }
        public ICommand AddItemCommand { get; }
        public ICommand GoToOrderDetailsCommand { get; }
        public ICommand GoToMenuEditionCommand { get; }
        public ICommand EditOrderStatusCommand { get; }

        public ObservableCollection<DishSectionGroupedModel> MenuGroupedBySection { get; set; } = new ObservableCollection<DishSectionGroupedModel>();

        public BackOfficeHomePageModel(BackOfficeRestaurantService backofficeService, FrontOfficeRestaurantService restaurantService, INavigationService navigationService, PageModelMessagingService messagingService, IAuthenticationService authenticationService)
        {
            Items = new ObservableCollection<Restaurant>();
            GoToMenuEditionCommand = CreateAsyncCommand<Restaurant>(GoToMenuEditionPage);
            LogoutCommand = CreateAsyncCommand(Logout);
            AddDishCommand = CreateAsyncCommand<string>(AddDish);
            AddDishSectionCommand = CreateAsyncCommand(AddDishSection);
            DeleteRestaurantCommand = CreateAsyncCommand(DeleteCurrentRestaurant);
            EditSectionCommand = CreateAsyncCommand<string>(EditSection);
            GoToEditRestaurantInfosCommand = CreateCommand(GoToEditRestaurantInfos);
            GoToEditDishCommand = CreateAsyncCommand<Dish>(EditDish);
            this.backOfficeService = backofficeService;
            AddItemCommand = CreateAsyncCommand(OnAddItem);
            GoToOrderDetailsCommand = CreateAsyncCommand<OrderVm>(GoToOrderDetails);
            this.restaurantService = restaurantService;
            this.navigationService = navigationService;
            this.messagingService = messagingService;
            this.authenticationService = authenticationService;
        }

        private Task GoToOrderDetails(OrderVm order)
        {
            return navigationService.GoToOrderDetails(MappingService.VmToModel(order));
        }

        private async Task GoToMenuEditionPage(Restaurant restaurant)
        {
            await navigationService.GoToMenuEdition(restaurant);
        }

        private void EditOrderStatus(OrderStatusEditionResult result)
        {
            if (result != null)
            {
                if (result.WasSuccessful)
                {
                    if (result.WasDeleted)
                    {
                        Orders.Remove(result.Order.ModelToVm());
                    }
                    else
                    {
                        var updateOrderIndex = Orders.IndexOf(result.Order.ModelToVm());
                        if (updateOrderIndex != -1)
                        {
                            Orders[updateOrderIndex].OrderStatus = result.ValidatedStatus;

                        }
                    }
                }
                else
                {
                    HandleError(result.ErrorMessage ?? "An error occured");

                }


            }


        }



        protected override void PostParamInitialization()
        {
            messagingService.Subscribe<OrderStatusEditionResult>("OrderStatusEdited", (edition) =>
                {
                    EditOrderStatus(edition);
                }, this);
        }

        public override async Task InitAsync()
        {
            try
            {
                CurrentRestaurant = await backOfficeService.GetUniqueRestaurantForAccount();
                var orders = await backOfficeService.GetOrdersForRestaurant(CurrentRestaurant.Id);

                if (orders != null)
                {
                    Orders = new ObservableCollection<OrderVm>(orders.Select(n => n.ModelToVm()));

                }
                CurrentLoggedAccount = authenticationService.LoggedUser?.RestaurantAdmin;


                await LoadMenu();
            }
            catch (RestaurantNotFoundException)
            {

                CurrentRestaurant = null;

                if (MenuGroupedBySection != null)
                {
                    MenuGroupedBySection.Clear();
                }

                throw;
            }
        }

        protected override Task Refresh()
        {
            return InitAsync();
        }


        public override Task CleanUp()
        {
            return Task.CompletedTask;
        }


        private async Task Logout()
        {
            messagingService.Unsubscribe<OrderStatusEditionResult>("OrderStatusEdited", this);

            await authenticationService.SignOut();

            await navigationService.GoToLanding();
        }


        public ICommand AddDishCommand { get; set; }
        public ICommand AddDishSectionCommand { get; set; }

        public ICommand DeleteRestaurantCommand { get; set; }
        public ICommand EditSectionCommand { get; }
        public ICommand GoToEditRestaurantInfosCommand { get; set; }

        public ICommand GoToEditDishCommand { get; set; }



        private async Task EditSection(string sectionName)
        {
            var result = await navigationService.GoToAddDishSection(new EditDishSectionParams() { Restaurant = CurrentRestaurant, DishSectionToEdit = CurrentRestaurant.Menu.Sections.SingleOrDefault(s => s.Name == sectionName) });

            if (result != null && result.WasSuccessful)
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
            var restaurantEdited = await navigationService.GoToEditRestaurantInfos(new Modal.RestaurantIdentity { Adresse = CurrentRestaurant.Adresse, Name = CurrentRestaurant.Name });

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
            var result = await navigationService.GoToAddDishSection(new EditDishSectionParams() { Restaurant = CurrentRestaurant });

            if (result != null)
            {
                if (result.WasSuccessful)
                {
                    await LoadMenu();
                }
                else
                {
                    HandleError(result.ErrorMessage ?? "An error occured");
                }
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

            await InitAsync();
        }


        public Restaurant CurrentRestaurant { get; set; }

        private async Task LoadMenu()
        {
            var currentRestaurant = await restaurantService.GetRestaurantById(CurrentRestaurant.Id);

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

        }


        private async Task OnAddItem()
        {
            await navigationService.GoToRestaurantEdition();
        }


    }
}
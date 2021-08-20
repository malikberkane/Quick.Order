using Quick.Order.AppCore;
using Quick.Order.AppCore.Authentication.Contracts;
using Quick.Order.AppCore.BusinessOperations;
using Quick.Order.AppCore.Contracts;
using Quick.Order.AppCore.Exceptions;
using Quick.Order.AppCore.Models;
using Quick.Order.Native.Services;
using Quick.Order.Native.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Quick.Order.Native.ViewModels
{
    public class BackOfficeHomePageModel : ExtendedPageModelBase
    {
        private readonly BackOfficeRestaurantService backOfficeService;
        private readonly IVibrationService vibrationService;
        private readonly FrontOfficeRestaurantService restaurantService;
        private readonly OrdersTrackingService ordersTrackingService;
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
        public ICommand GenerateQrCodeCommand { get; }
        public ICommand EditOrderStatusCommand { get; }

        public DishSectionGroupedModelCollection MenuGroupedBySection { get; set; } = new DishSectionGroupedModelCollection();

        public BackOfficeHomePageModel(BackOfficeRestaurantService backofficeService, IVibrationService vibrationService, FrontOfficeRestaurantService restaurantService, OrdersTrackingService ordersTrackingService, INavigationService navigationService, PageModelMessagingService messagingService, IAuthenticationService authenticationService)
        {
            Items = new ObservableCollection<Restaurant>();
            GoToMenuEditionCommand = CreateAsyncCommand<Restaurant>(GoToMenuEditionPage);
            GenerateQrCodeCommand = CreateAsyncCommand(GoToQrCodeGeneration);
            LogoutCommand = CreateAsyncCommand(Logout);
            AddDishCommand = CreateCommand<string>(AddDish);
            ReloadMenuCommand = CreateAsyncCommand(LoadMenu);
            AddDishSectionCommand = CreateCommand(AddDishSection);
            DeleteRestaurantCommand = CreateAsyncCommand(DeleteCurrentRestaurant);
            EditSectionCommand = CreateCommand<string>(EditSection);
            GoToEditRestaurantInfosCommand = CreateCommand(GoToEditRestaurantInfos);
            GoToEditDishCommand = CreateCommand<Dish>(EditDish);
            this.backOfficeService = backofficeService;
            this.vibrationService = vibrationService;
            AddItemCommand = CreateCommand(AddRestaurant);
            GoToOrderDetailsCommand = CreateAsyncCommand<OrderVm>(GoToOrderDetails);
            this.restaurantService = restaurantService;
            this.ordersTrackingService = ordersTrackingService;
            this.navigationService = navigationService;
            this.messagingService = messagingService;
            this.authenticationService = authenticationService;
        }

        private Task GoToQrCodeGeneration()
        {
            return navigationService.GoToQrGeneration(CurrentRestaurant);
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

                Orders = new ObservableCollection<OrderVm>();

                CurrentLoggedAccount = authenticationService.LoggedUser?.RestaurantAdmin;


                await LoadMenu();

                ordersTrackingService.StartOrdersTracking();
                ordersTrackingService.OrderListChanged += OrdersTrackingService_OrderListChanged;
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

        private void OrdersTrackingService_OrderListChanged(object sender, OrdersChangedEventArgs args)
        {
            try
            {
                if (args.ListDifferences.RemovedItems != null)
                {
                    foreach (var removedItem in args.ListDifferences.RemovedItems)
                    {
                        Orders.Remove(removedItem.ModelToVm());

                    }
                }

                if (args.ListDifferences.RemovedItems != null && args.ListDifferences.NewItems.Any())
                {
                    vibrationService.Vibrate();
                    foreach (var addedItems in args.ListDifferences.NewItems)
                    {
                        Orders.Insert(0, addedItems.ModelToVm());

                    }
                }
            }
            catch (System.Exception ex)
            {

                OnExceptionCaught(ex);
            }
            
        }

        public override Task CleanUp()
        {
            messagingService.Unsubscribe<OrderStatusEditionResult>("OrderStatusEdited", this);

            ordersTrackingService.StopOrderTracking();
            ordersTrackingService.OrderListChanged -= OrdersTrackingService_OrderListChanged;
            return Task.CompletedTask;
        }


        private async Task Logout()
        {

            await this.CleanUp();
            await authenticationService.SignOut();

            await navigationService.GoToLanding();
        }


        public ICommand AddDishCommand { get; set; }
        public ICommand ReloadMenuCommand { get; }
        public ICommand AddDishSectionCommand { get; set; }

        public ICommand DeleteRestaurantCommand { get; set; }
        public ICommand EditSectionCommand { get; }
        public ICommand GoToEditRestaurantInfosCommand { get; set; }

        public ICommand GoToEditDishCommand { get; set; }



        private Task EditSection(string sectionName)
        {
            return navigationService.GoToAddDishSection(new EditDishSectionParams() { MenuGroupedBySection = MenuGroupedBySection, Restaurant = CurrentRestaurant, DishSectionToEdit = CurrentRestaurant.Menu.Sections.SingleOrDefault(s => s.Name == sectionName) });

        }

        private async Task EditDish(Dish dishToEdit)
        {
            var result = await navigationService.GoToEditDish(new EditDishParams { Dish = dishToEdit, Restaurant = CurrentRestaurant });

            if (result != null && result.WasSuccessful)
            {
                if (result.DeletedDish != null)
                {
                    MenuGroupedBySection.RemoveDish(result.DeletedDish);
                }
                else
                {
                    MenuGroupedBySection.UpdateDish(dishToEdit, result.EditedDish);
                }
            }

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
            var result = await navigationService.GoToAddDishSection(new EditDishSectionParams() { Restaurant = CurrentRestaurant, MenuGroupedBySection = MenuGroupedBySection });

            if (result != null)
            {
                if (result.WasSuccessful)
                {
                    return;
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

            var navParams = new AddDishParams { Restaurant = CurrentRestaurant, Section = section };

            var result = await navigationService.GoToAddDish(navParams);

            if (result != null && result.WasSuccessful)
            {
                if (result.AddedDish != null)
                {
                    MenuGroupedBySection.AddDishToSection(section.Name, result.AddedDish);
                }

            }

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

            CreateMenuList(currentRestaurant.Menu);

        }

        private void CreateMenuList(Menu menu)
        {
            if (MenuGroupedBySection != null)
            {
                MenuGroupedBySection.Clear();
            }

            foreach (var section in menu.Sections)
            {
                var newSection = new DishSectionGroupedModel { SectionName = section.Name };


                foreach (var dish in section.Dishes)
                {
                    newSection.Add(dish);
                }

                MenuGroupedBySection.Add(newSection);
            }
        }

        private async Task AddRestaurant()
        {
            var editedRestaurant = await navigationService.GoToRestaurantEdition();

            if (editedRestaurant != null)
            {
                CurrentRestaurant = editedRestaurant;
                CreateMenuList(editedRestaurant.Menu);

            }
        }


    }
}
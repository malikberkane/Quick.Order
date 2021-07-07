using MalikBerkane.MvvmToolkit;
using Quick.Order.AppCore.Authentication.Contracts;
using Quick.Order.AppCore.BusinessOperations;
using Quick.Order.AppCore.Models;
using Quick.Order.Native.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Quick.Order.Native.ViewModels
{
    public class BackOfficeHomePageModel : PageModelBase
    {
        private readonly BackOfficeRestaurantService restaurantService;
        private readonly INavigationService navigationService;
        private readonly PageModelMessagingService messagingService;
        private readonly IAuthenticationService authenticationService;

        public ObservableCollection<Restaurant> Items { get; }

        public List<OrderVm> Orders { get; set; }

        public RestaurantAdmin CurrentLoggedAccount { get; set; }
        public ICommand LogoutCommand { get; }

        public ICommand LoadItemsCommand { get; }
        public ICommand AddItemCommand { get; }
        public ICommand GoToMenuEditionCommand { get; }
        public ICommand EditOrderStatusCommand { get; }


        public BackOfficeHomePageModel(BackOfficeRestaurantService restaurantService, INavigationService navigationService, PageModelMessagingService messagingService, IAuthenticationService authenticationService)
        {
            Items = new ObservableCollection<Restaurant>();
            LoadItemsCommand = new AsyncCommand(async () => await ExecuteLoadItemsCommand());

            GoToMenuEditionCommand = CreateAsyncCommand<Restaurant>(GoToMenuEditionPage);
            LogoutCommand = CreateAsyncCommand(Logout);
            EditOrderStatusCommand = CreateAsyncCommand<OrderVm>(EditOrderStatus, setPageModelToLoadingState: false);

            AddItemCommand = new AsyncCommand(OnAddItem);
            this.restaurantService = restaurantService;
            this.navigationService = navigationService;
            this.messagingService = messagingService;
            this.authenticationService = authenticationService;
        }

        private async Task GoToMenuEditionPage(Restaurant restaurant)
        {
            await navigationService.GoToMenuEdition(restaurant);
        }

        private async Task EditOrderStatus(OrderVm order)
        {
            var result = await navigationService.GoToEditOrderStatus(order.VmToModel());
            if (result!=null && result.WasSuccessful)
            {
                var updateOrderIndex = Orders.IndexOf(order);
                if (updateOrderIndex != -1)
                {
                    Orders[updateOrderIndex].OrderStatus = result.ValidatedStatus;
                    
                }
            }

        }

        async Task ExecuteLoadItemsCommand()
        {

            Items.Clear();
            var items = await restaurantService.GetAllRestaurantsForAccount();
            foreach (var item in items)
            {
                Items.Add(item);
            }

        }


        public override async Task InitAsync()
        {
            await ExecuteLoadItemsCommand();
            var orders = await restaurantService.GetOrdersForRestaurant(System.Guid.Parse("06b565f4-ef11-4839-a551-8e5bdf0cca2f"));

            if (orders != null)
            {
                Orders = orders.Select(n => n.ModelToVm()).ToList();

            }
            CurrentLoggedAccount = authenticationService.LoggedUser?.RestaurantAdmin;
        }

        protected override Task Refresh()
        {
            return EnsurePageModelIsInLoadingState(ExecuteLoadItemsCommand);
        }


        public override Task CleanUp()
        {
            return Task.CompletedTask;
        }


        private async Task OnAddItem()
        {
            await navigationService.GoToRestaurantEdition();
        }


        private async Task Logout()
        {
            await authenticationService.SignOut();

            await navigationService.GoToLanding();
        }

    }
}
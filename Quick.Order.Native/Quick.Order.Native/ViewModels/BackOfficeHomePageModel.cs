using MalikBerkane.MvvmToolkit;
using Quick.Order.AppCore.Authentication.Contracts;
using Quick.Order.AppCore.BusinessOperations;
using Quick.Order.AppCore.Models;
using Quick.Order.Native.Services;
using System.Collections.ObjectModel;
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

        public RestaurantAdmin CurrentLoggedAccount { get; set; }
        public ICommand LogoutCommand { get; }

        public ICommand LoadItemsCommand { get; }
        public ICommand AddItemCommand { get; }
        public ICommand GoToMenuEditionCommand { get; }

        public BackOfficeHomePageModel(BackOfficeRestaurantService restaurantService, INavigationService navigationService, PageModelMessagingService messagingService, IAuthenticationService authenticationService)
        {
            Items = new ObservableCollection<Restaurant>();
            LoadItemsCommand = new AsyncCommand(async () => await ExecuteLoadItemsCommand());

            GoToMenuEditionCommand = new AsyncCommand<Restaurant>(GoToMenuEditionPage);
            LogoutCommand = new AsyncCommand(Logout);

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
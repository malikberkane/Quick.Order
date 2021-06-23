using MalikBerkane.MvvmToolkit;
using Quick.Order.AppCore.BusinessOperations;
using Quick.Order.AppCore.Models;
using Quick.Order.Native.Services;
using Quick.Order.Native.Views;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Quick.Order.Native.ViewModels
{
    public class BackOfficeHomePageModel : PageModelBase
    {
        private readonly BackOfficeRestaurantService restaurantService;
        private readonly INavigationService navigationService;
        private readonly PageModelMessagingService messagingService;

        public ObservableCollection<Restaurant> Items { get; }
        public ICommand LoadItemsCommand { get; }
        public ICommand AddItemCommand { get; }
        public ICommand GoToMenuEditionCommand { get; }

        public BackOfficeHomePageModel(BackOfficeRestaurantService restaurantService, INavigationService navigationService, PageModelMessagingService messagingService)
        {
            Items = new ObservableCollection<Restaurant>();
            LoadItemsCommand = new AsyncCommand(async () => await ExecuteLoadItemsCommand());

            GoToMenuEditionCommand = new AsyncCommand<Restaurant>(GoToMenuEditionPage);

            AddItemCommand = new AsyncCommand(OnAddItem);
            this.restaurantService = restaurantService;
            this.navigationService = navigationService;
            this.messagingService = messagingService;
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


        public override Task InitAsync()
        {
            messagingService.Subscribe("RestaurantAdded", async () =>
            {
                await ExecuteLoadItemsCommand();
            }, this);

            return ExecuteLoadItemsCommand();
        }


        public override Task CleanUp()
        {
            return Task.Delay(100);
        }


        private async Task OnAddItem()
        {
            await navigationService.GoToRestaurantEdition();
        }

      
    }
}
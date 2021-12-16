using Quick.Order.AppCore.Exceptions;
using Quick.Order.AppCore.Resources;
using Quick.Order.Native.ViewModels.Base;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Quick.Order.Native.ViewModels
{
    public class LandingPageModel : ExtendedPageModelBase<string>
    {
      
        public ICommand GoToSignInCommand { get; }

        public ICommand ScanQrCommand { get; }
        public ICommand DiscoverCommand { get; }
        public ICommand ContinueOrderInProgressCommand { get; }

        public AppCore.Models.Order OrderInProgress { get; set; }
        public LandingPageModel()
        {
            GoToSignInCommand = CreateAsyncCommand(GoToSignIn);
            ScanQrCommand = CreateAsyncCommand(ScanQr);
            DiscoverCommand = CreateAsyncCommand(Discover);
            ContinueOrderInProgressCommand = CreateCommand(ContinueOrderInProgress);
        }

        private Task Discover()
        {
            return NavigationService.Order.GoToDiscover();
        }

        private Task GoToSignIn()
        {
            return NavigationService.SignIn.GoToLogin();
        }

        private async Task ScanQr()
        {
            var result = await NavigationService.Order.GoToQrCodeScanningModal();

            if (result == null)
            {
                return;
            }

            var idFromRestaurantLink = ServicesAggregate.Plugin.DeepLink.ExtractRestaurantIdFromUri(new Uri(result));

            if (Guid.TryParse(idFromRestaurantLink, out Guid restaurantId))
            {
                var restaurant = await ServicesAggregate.Repositories.Restaurants.GetById(restaurantId);

                if (restaurant == null)
                {
                    throw new RestaurantNotFoundException();
                }
                await NavigationService.Order.GoToMenu(restaurant);
            }
            else
            {
                throw new InvalidRestaurantCode();
            }
        }


        protected override Task Refresh()
        {
            OrderInProgress = ServicesAggregate.Business.LocalHistory.GetLocalOrder() ;

            return Task.CompletedTask;
        }
        public override async Task InitAsync()
        {
            OrderInProgress = ServicesAggregate.Business.LocalHistory.GetLocalOrder();

            if (Parameter != null)
            {
                if (Guid.TryParse(Parameter, out Guid restaurantId))
                {
                    var restaurant = await ServicesAggregate.Repositories.Restaurants.GetById(restaurantId);

                    if (restaurant == null)
                    {
                        throw new RestaurantNotFoundException();
                    }
                    await NavigationService.Order.GoToMenu(restaurant);
                }
                else
                {
                    throw new InvalidRestaurantCode();
                }
            }
          
        }

        private async Task ContinueOrderInProgress()
        {
            var choice = await NavigationService.Common.PromptForConfirmation($" {AppResources.OrderPending}: {OrderInProgress.OrderedItems.First().Dish.Name} etc..", AppResources.Continue, AppResources.Cancel, isDestructive: false);

            if (choice)
            {
                var restaurant = await ServicesAggregate.Repositories.Restaurants.GetById(OrderInProgress.RestaurantId);

                if (restaurant == null)
                {
                    throw new RestaurantNotFoundException();
                }
                await NavigationService.Order.GoToMenu(restaurant);
            }
            else
            {
                ServicesAggregate.Business.LocalHistory.DeleteLocalOrder();
                OrderInProgress = null;
            }
        }
    }
}

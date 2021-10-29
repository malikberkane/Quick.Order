using MalikBerkane.MvvmToolkit;
using Quick.Order.AppCore.BusinessOperations;
using Quick.Order.AppCore.Contracts;
using Quick.Order.AppCore.Exceptions;
using Quick.Order.Native.Services;
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

        public LandingPageModel()
        {
            GoToSignInCommand = CreateAsyncCommand(GoToSignIn);
            ScanQrCommand = CreateAsyncCommand(ScanQr);
            DiscoverCommand = CreateAsyncCommand(Discover);
        
        }

        private Task Discover()
        {
            return NavigationService.GoToDiscover();
        }

        private Task GoToSignIn()
        {
            return NavigationService.GoToLogin();
        }

        private async Task ScanQr()
        {


            var result = await NavigationService.GoToQrCodeScanningModal();

            if (result == null)
            {
                return;
            }
            //var result = "867d26a7-1ef8-4944-beb8-1b3bddb0c091";

            var idFromRestaurantLink = ServicesAggregate.Plugin.DeepLink.ExtractRestaurantIdFromUri(new Uri(result));

            if (Guid.TryParse(idFromRestaurantLink, out Guid restaurantId))
            {
                var restaurant = await ServicesAggregate.Repositories.Restaurants.GetById(restaurantId);

                if (restaurant == null)
                {
                    throw new RestaurantNotFoundException();
                }
                await NavigationService.GoToMenu(restaurant);
            }
            else
            {
                throw new InvalidRestaurantCode();
            }
        }



        public override async Task InitAsync()
        {
            if (Parameter != null)
            {
                if (Guid.TryParse(Parameter, out Guid restaurantId))
                {
                    var restaurant = await ServicesAggregate.Repositories.Restaurants.GetById(restaurantId);

                    if (restaurant == null)
                    {
                        throw new RestaurantNotFoundException();
                    }
                    await NavigationService.GoToMenu(restaurant);
                }
                else
                {
                    throw new InvalidRestaurantCode();
                }
            }
            else
            {
                if (ServicesAggregate.Business.LocalHistory.GetLocalOrder() is AppCore.Models.Order order)
                {
                    IsLoading = false;
                    var choice=await NavigationService.PromptForConfirmation("Commande en cours", $"Vous avez commencé une commande ({order.OrderedItems.First().Dish.Name} etc..)", "Continuer", "Abandonner");
               
                    if(choice)
                    {
                        var restaurant = await ServicesAggregate.Repositories.Restaurants.GetById(order.RestaurantId);

                        if (restaurant == null)
                        {
                            throw new RestaurantNotFoundException();
                        }
                        await NavigationService.GoToMenu(restaurant);
                    }
                    else
                    {
                        ServicesAggregate.Business.LocalHistory.DeleteLocalOrder();
                    }
                }
                
            }
        }
    }
}

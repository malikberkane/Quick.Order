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
    public class LandingViewModel : ExtendedPageModelBase<string>
    {
        private readonly ILocalHistoryService localHistoryService;
        private readonly FrontOfficeRestaurantService restaurantService;
        private readonly IDeepLinkService deepLinkService;

        public ICommand GoToSignInCommand { get; }

        public ICommand ScanQrCommand { get; }
        public ICommand DiscoverCommand { get; }
        public INavigationService navigationService { get; }

        public LandingViewModel(INavigationService navigationService, ILocalHistoryService localHistoryService, FrontOfficeRestaurantService restaurantService, IDeepLinkService deepLinkService)
        {
            GoToSignInCommand = CreateAsyncCommand(GoToSignIn);
            ScanQrCommand = CreateAsyncCommand(ScanQr);
            DiscoverCommand = CreateAsyncCommand(Discover);
            this.navigationService = navigationService;
            this.localHistoryService = localHistoryService;
            this.restaurantService = restaurantService;
            this.deepLinkService = deepLinkService;
        }

        private Task Discover()
        {
            return navigationService.GoToDiscover();
        }

        private Task GoToSignIn()
        {
            return navigationService.GoToLogin();
        }

        private async Task ScanQr()
        {


            var result = await navigationService.GoToQrCodeScanningModal();

            if (result == null)
            {
                return;
            }
            //var result = "867d26a7-1ef8-4944-beb8-1b3bddb0c091";

            var idFromRestaurantLink = deepLinkService.ExtractRestaurantIdFromUri(new Uri(result));

            if (Guid.TryParse(idFromRestaurantLink, out Guid restaurantId))
            {
                var restaurant = await restaurantService.GetRestaurantById(restaurantId);

                if (restaurant == null)
                {
                    throw new RestaurantNotFoundException();
                }
                await navigationService.GoToMenu(restaurant);
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
                    var restaurant = await restaurantService.GetRestaurantById(restaurantId);

                    if (restaurant == null)
                    {
                        throw new RestaurantNotFoundException();
                    }
                    await navigationService.GoToMenu(restaurant);
                }
                else
                {
                    throw new InvalidRestaurantCode();
                }
            }
            else
            {
                if (localHistoryService.GetLocalOrder() is AppCore.Models.Order order)
                {
                    var choice=await navigationService.PromptForConfirmation("Commande en cours", $"Vous avez commencé une commande ({order.OrderedItems.First().Dish.Name} etc..)", "Continuer", "Abandonner");
               
                    if(choice)
                    {
                        var restaurant = await restaurantService.GetRestaurantById(order.RestaurantId);

                        if (restaurant == null)
                        {
                            throw new RestaurantNotFoundException();
                        }
                        await navigationService.GoToMenu(restaurant);
                    }
                    else
                    {
                        localHistoryService.DeleteLocalOrder();
                    }
                }
                
            }
        }
    }
}

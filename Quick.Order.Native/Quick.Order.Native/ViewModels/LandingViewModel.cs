using MalikBerkane.MvvmToolkit;
using Quick.Order.AppCore.BusinessOperations;
using Quick.Order.AppCore.Exceptions;
using Quick.Order.Native.Services;
using Quick.Order.Native.ViewModels.Base;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Quick.Order.Native.ViewModels
{
    public class LandingViewModel : ExtendedPageModelBase<string>
    {
        private readonly FrontOfficeRestaurantService restaurantService;

        public ICommand GoToSignInCommand { get; }

        public ICommand ScanQrCommand { get; }

        public INavigationService navigationService { get; }

        public LandingViewModel(INavigationService navigationService, FrontOfficeRestaurantService restaurantService)
        {
            GoToSignInCommand = CreateAsyncCommand(GoToSignIn);
            ScanQrCommand = CreateAsyncCommand(ScanQr);
            this.navigationService = navigationService;
            this.restaurantService = restaurantService;
        }

        private Task GoToSignIn()
        {
            return navigationService.GoToLogin();
        }

        private async Task ScanQr()
        {




            //var result= await navigationService.GoToQrCodeScanningModal();

            // if (result == null)
            // {
            //     return;
            // }


            var result = "867d26a7-1ef8-4944-beb8-1b3bddb0c091";

            if (Guid.TryParse(result, out Guid restaurantId))
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
        }
    }

}

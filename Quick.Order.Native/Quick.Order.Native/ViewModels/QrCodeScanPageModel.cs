using GoogleVisionBarCodeScanner;
using MalikBerkane.MvvmToolkit;
using Quick.Order.AppCore.BusinessOperations;
using Quick.Order.AppCore.Exceptions;
using Quick.Order.Native.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Quick.Order.Native.ViewModels
{
    public class QrCodeScanPageModel : PageModelBase
    {
        private readonly INavigationService navigationService;
        private readonly FrontOfficeRestaurantService restaurantService;

        public QrCodeScanPageModel(INavigationService navigationService, FrontOfficeRestaurantService restaurantService)
        {
            this.navigationService = navigationService;
            this.restaurantService = restaurantService;
            GoToMenuCommand = CreateAsyncCommand<string>(GoToMenu);

        }
        public ICommand GoToMenuCommand { get; set; }

        private async Task GoToMenu(string arg)
        {
            if(Guid.TryParse(arg, out Guid restaurantId))
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
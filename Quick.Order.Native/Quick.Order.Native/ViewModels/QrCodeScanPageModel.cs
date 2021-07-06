using GoogleVisionBarCodeScanner;
using MalikBerkane.MvvmToolkit;
using Quick.Order.AppCore.BusinessOperations;
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
            GoToMenuCommand = new AsyncCommand<string>(GoToMenu);

        }
        public ICommand GoToMenuCommand { get; set; }
        
        private async Task GoToMenu(string arg)
        {
            await EnsurePageModelIsInLoadingState(async () =>
            {
                var restaurant = await restaurantService.GetRestaurantById(System.Guid.Parse(arg));

                if (restaurant == null)
                {
                    throw new System.Exception("restaurant not found");
                }
                await navigationService.GoToMenu(restaurant);
            });
           
        }
    }


}
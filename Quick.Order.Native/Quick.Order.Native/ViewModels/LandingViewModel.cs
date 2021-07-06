using MalikBerkane.MvvmToolkit;
using Quick.Order.AppCore.BusinessOperations;
using Quick.Order.Native.Services;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Quick.Order.Native.ViewModels
{
    public class LandingViewModel : PageModelBase
    {
        private readonly FrontOfficeRestaurantService restaurantService;

        public ICommand GoToSignInCommand { get; }

        public ICommand ScanQrCommand { get; }

        public INavigationService navigationService { get; }

        public LandingViewModel(INavigationService navigationService, FrontOfficeRestaurantService restaurantService)
        {
            GoToSignInCommand = new AsyncCommand(GoToSignIn);
            ScanQrCommand = new AsyncCommand(ScanQr);
            this.navigationService = navigationService;
            this.restaurantService = restaurantService;
        }

        private Task GoToSignIn()
        {
            return navigationService.GoToLogin();
        }

        private async Task ScanQr()
        {

            await navigationService.GoToQrCodeScanning();
           
        }
    }

}

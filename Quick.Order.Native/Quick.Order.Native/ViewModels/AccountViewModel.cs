using MalikBerkane.MvvmToolkit;
using Quick.Order.AppCore.Authentication.Contracts;
using Quick.Order.Native.Views;
using System.Windows.Input;
using Xamarin.Forms;

namespace Quick.Order.Native.ViewModels
{
    public class AccountViewModel : PageModelBase
    {
        public ICommand LogoutCommand { get; }

        private readonly IAuthenticationService authenticationService;

        public AccountViewModel(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;

            LogoutCommand = new Command(Logout);

        }

        private async void Logout()
        {
            await authenticationService.SignOut();

            Application.Current.MainPage= new NavigationPage(new LandingPage());
        }
    }
}

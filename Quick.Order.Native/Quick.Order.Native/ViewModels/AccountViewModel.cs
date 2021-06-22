using Quick.Order.AppCore.Authentication.Contracts;
using Quick.Order.Native.Views;
using System;
using Xamarin.Forms;

namespace Quick.Order.Native.ViewModels
{
    public class AccountViewModel : BaseViewModel
    {
        public Command LogoutCommand { get; }

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

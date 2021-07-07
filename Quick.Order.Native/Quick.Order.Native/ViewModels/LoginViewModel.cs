using MalikBerkane.MvvmToolkit;
using Quick.Order.AppCore.Authentication.Contracts;
using Quick.Order.AppCore.Models;
using Quick.Order.Native.Services;
using Quick.Order.Native.Views;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Quick.Order.Native.ViewModels
{
    public class LoginViewModel : PageModelBase
    {
        private readonly IAuthenticationService authenticationService;
        private readonly INavigationService navigationService;

        public LoginViewModel(IAuthenticationService authenticationService, INavigationService navigationService)
        {
            this.authenticationService = authenticationService;
            this.navigationService = navigationService;
            LoginCommand = CreateAsyncCommand(OnLoginClicked);

            GoToCreateUserCommand = new Command(CreateUser);
        }
        public ICommand LoginCommand { get; }
        public ICommand GoToCreateUserCommand { get; }


        public string LoginText { get; set; }
        public string PasswordText { get; set; }

        public override Task InitAsync()
        {
#if DEBUG
            LoginText = "tarektebtal@gmail.com";
            PasswordText = "123456";

#endif
            return base.InitAsync();
        }
        private async Task OnLoginClicked()
        {

            try
            {

                AutenticatedRestaurantAdmin autenticatedRestaurantAdmin = null;

               
                autenticatedRestaurantAdmin = await authenticationService.SignIn(LoginText, PasswordText);

                if (autenticatedRestaurantAdmin?.AuthenticationToken != null)
                {
                    await navigationService.GoToMainBackOffice();

                }

            }
            catch (Exception ex)
            {

                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "ok");
            }
        }


        private void CreateUser()
        {
            Application.Current.MainPage.Navigation.PushAsync(new CreateUserPage());

        }
    }

}

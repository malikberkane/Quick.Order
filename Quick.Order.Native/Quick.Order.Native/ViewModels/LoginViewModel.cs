using Quick.Order.AppCore.Authentication.Contracts;
using Quick.Order.Native.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Quick.Order.Native.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IAuthenticationService authenticationService;

        public LoginViewModel(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;

            LoginCommand = new Command(OnLoginClicked);

            GoToCreateUserCommand = new Command(CreateUser);
        }
        public Command LoginCommand { get; }
        public Command GoToCreateUserCommand { get; }


        public string LoginText { get; set; }
        public string PasswordText { get; set; }


        private async void OnLoginClicked(object obj)
        {

            try
            {
                var authResult = await authenticationService.SignIn(LoginText, PasswordText);

                if (authResult?.AuthenticationToken != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Success", authResult.AuthenticationToken, "ok");
                    Application.Current.MainPage = new AppShell();

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

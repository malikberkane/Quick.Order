using Quick.Order.AppCore.Authentication.Contracts;
using System;
using Xamarin.Forms;

namespace Quick.Order.Native.ViewModels
{
    public class CreateUserViewModel : BaseViewModel
    {
        public Command CreateUserCommand { get; }


        private readonly IAuthenticationService authenticationService;
        public string NewUserText { get; set; }
        public string NewUserEmail { get; set; }

        public string NewUserPassword { get; set; }

        public CreateUserViewModel(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;


            CreateUserCommand = new Command(CreateUser);
        }

        private async void CreateUser()
        {
            try
            {
                var authResult = await authenticationService.CreateUser(NewUserText, NewUserEmail, NewUserPassword);

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

    }

}

using MalikBerkane.MvvmToolkit;
using Quick.Order.AppCore.Authentication.Contracts;
using Quick.Order.AppCore.Models;
using Quick.Order.Native.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Quick.Order.Native.ViewModels
{
    public class CreateUserViewModel : PageModelBase
    {
        public ICommand CreateUserCommand { get; }


        private readonly IAuthenticationService authenticationService;
        private readonly INavigationService navigationService;

        public string NewUserText { get; set; }
        public string NewUserEmail { get; set; }

        public string NewUserPassword { get; set; }

        public CreateUserViewModel(IAuthenticationService authenticationService, INavigationService navigationService)
        {
            this.authenticationService = authenticationService;
            this.navigationService = navigationService;
            CreateUserCommand = new AsyncCommand(CreateUser);
        }

        private async Task CreateUser()
        {
            try
            {

                AutenticatedRestaurantAdmin autenticatedRestaurantAdmin = null;

                await EnsurePageModelIsInLoadingState(async () =>
                {
                    autenticatedRestaurantAdmin = await authenticationService.CreateUser(NewUserText, NewUserEmail, NewUserPassword);
                });


                if (autenticatedRestaurantAdmin?.AuthenticationToken != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Success", autenticatedRestaurantAdmin.AuthenticationToken, "ok");
                    await navigationService.GoToMainBackOffice();
                }

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "ok");
            }
        }

    }

}

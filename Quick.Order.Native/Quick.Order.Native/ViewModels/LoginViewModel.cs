using MalikBerkane.MvvmToolkit;
using Quick.Order.AppCore.Authentication.Contracts;
using Quick.Order.AppCore.BusinessOperations;
using Quick.Order.AppCore.Exceptions;
using Quick.Order.AppCore.Models;
using Quick.Order.Native.Services;
using Quick.Order.Native.ViewModels.Base;
using Quick.Order.Native.Views;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Quick.Order.Native.ViewModels
{
    public class LoginViewModel : ExtendedPageModelBase
    {
        private readonly IAuthenticationService authenticationService;
        private readonly INavigationService navigationService;
        private readonly BackOfficeSessionService backOfficeSessionService;

        public LoginViewModel(IAuthenticationService authenticationService, INavigationService navigationService, BackOfficeSessionService backOfficeSessionService)
        {
            this.authenticationService = authenticationService;
            this.navigationService = navigationService;
            this.backOfficeSessionService = backOfficeSessionService;
            LoginCommand = CreateAsyncCommand(OnLoginClicked);
            GoogleLoginCommand = CreateAsyncCommand(GoogleLogin);
            GoToCreateUserCommand = CreateCommand(CreateUser);
        }

        private async Task GoogleLogin()
        {
            var autenticatedRestaurantAdmin = await authenticationService.SignInWithOAuth();

            if (autenticatedRestaurantAdmin.AuthenticationToken == null)
            {
                throw new Exception("Empty token");
            }

            if (autenticatedRestaurantAdmin?.RestaurantAdmin != null)
            {
                await backOfficeSessionService.SetRestaurantForSession(autenticatedRestaurantAdmin.RestaurantAdmin);
            }

            await navigationService.GoToMainBackOffice();

        }

        public ICommand LoginCommand { get; }
        public ICommand GoogleLoginCommand { get; }
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

           var autenticatedRestaurantAdmin = await authenticationService.SignIn(LoginText, PasswordText);

            if (autenticatedRestaurantAdmin.AuthenticationToken == null)
            {
                throw new Exception("Empty token");
            }

            if (autenticatedRestaurantAdmin?.RestaurantAdmin != null)
            {
                await backOfficeSessionService.SetRestaurantForSession(autenticatedRestaurantAdmin.RestaurantAdmin);
            }

            await navigationService.GoToMainBackOffice();



        }


        private Task CreateUser()
        {
            return Application.Current.MainPage.Navigation.PushAsync(new CreateUserPage());

        }
    }

}

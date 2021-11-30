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
using Quick.Order.AppCore.Resources;

using System.Windows.Input;
using Xamarin.Forms;

namespace Quick.Order.Native.ViewModels
{
    public class LoginPageModel : ExtendedPageModelBase
    {
      
        public LoginPageModel()
        {
            LoginCommand = CreateAsyncCommand(OnLoginClicked);
            GoogleLoginCommand = CreateAsyncCommand(GoogleLogin);
            GoToCreateUserCommand = CreateCommand(NavigationService.SignIn.GoToCreateUser);
            ForgotPasswordCommand = CreateAsyncCommand(SendPasswordResetEmail);
        }

        private  async Task SendPasswordResetEmail()
        {
            await ServicesAggregate.Business.Authentication.SendPasswordResetEmail(LoginText);

            AlertUserService.ShowSnack(AppResources.ResetPasswordEmailAlert);
        }

        private async Task GoogleLogin()
        {
            var autenticatedRestaurantAdmin = await ServicesAggregate.Business.Authentication.SignInWithOAuth();

            if (autenticatedRestaurantAdmin.AuthenticationToken == null)
            {
                throw new Exception("Empty token");
            }

            if (autenticatedRestaurantAdmin?.RestaurantAdmin != null)
            {
                await ServicesAggregate.Business.BackOfficeSession.SetRestaurantForSession(autenticatedRestaurantAdmin.RestaurantAdmin);
            }

            await NavigationService.BackOffice.GoToMainBackOffice();

        }

        public ICommand LoginCommand { get; }
        public ICommand GoogleLoginCommand { get; }
        public ICommand GoToCreateUserCommand { get; }
        public ICommand ForgotPasswordCommand { get; }

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

           var autenticatedRestaurantAdmin = await ServicesAggregate.Business.Authentication.SignIn(LoginText, PasswordText);

            if (autenticatedRestaurantAdmin.AuthenticationToken == null)
            {
                throw new Exception("Empty token");
            }

            if (autenticatedRestaurantAdmin?.RestaurantAdmin != null)
            {
                await ServicesAggregate.Business.BackOfficeSession.SetRestaurantForSession(autenticatedRestaurantAdmin.RestaurantAdmin);
            }

            await NavigationService.BackOffice.GoToMainBackOffice();



        }

    }

}

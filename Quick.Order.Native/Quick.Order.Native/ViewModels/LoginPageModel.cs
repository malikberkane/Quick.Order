using Quick.Order.Native.ViewModels.Base;
using System;
using System.Threading.Tasks;
using Quick.Order.AppCore.Resources;

using System.Windows.Input;

namespace Quick.Order.Native.ViewModels
{
    public class LoginPageModel : ExtendedPageModelBase
    {
      
        public LoginPageModel()
        {
            LoginCommand = CreateAsyncCommand(Login);
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

        public string LoginText { get; set; } /*= "johnny@gmail.com";*/
        public string PasswordText { get; set; } /*= "123456";*/


        private async Task Login()
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


        protected override void PostParamInitialization()
        {
            LoginText = ServicesAggregate.Business.LocalHistory.GetUserEmail();
        }

    }

}

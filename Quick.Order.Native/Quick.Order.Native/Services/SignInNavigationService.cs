using Quick.Order.Native.ViewModels;
using Quick.Order.Native.Views;
using System.Threading.Tasks;

namespace Quick.Order.Native.Services
{
    public class SignInNavigationService: BaseNavigationService, ISignInNavigation
    {
        public Task GoToLogin()
        {
             viewModelNavigationService.CreateNavigationRoot<LoginPage, LoginPageModel>();

            return Task.CompletedTask;
     
        }
        public Task GoToLanding(string scannedCode = null)
        {

            viewModelNavigationService.CreateNavigationRoot<LandingPage, LandingPageModel>(scannedCode);
            return Task.CompletedTask;

        }

        public Task GoToCreateUser()
        {
            return viewModelNavigationService.PushPage<CreateUserPage, CreateUserPageModel>();
        }

    }
}

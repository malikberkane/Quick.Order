using MalikBerkane.MvvmToolkit;
using Quick.Order.Native.Services;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Quick.Order.Native.ViewModels
{
    public class LandingViewModel : PageModelBase
    {

        public ICommand GoToSignInCommand { get; }
        public INavigationService navigationService { get; }

        public LandingViewModel(INavigationService navigationService)
        {
            GoToSignInCommand = new AsyncCommand(GoToSignIn);
            this.navigationService = navigationService;
        }

        private Task GoToSignIn()
        {
            return navigationService.GoToLogin();
        }
    }

}

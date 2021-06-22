using Quick.Order.Native.Views;
using Xamarin.Forms;

namespace Quick.Order.Native.ViewModels
{
    public class LandingViewModel : BaseViewModel
    {

        public Command GoToSignInCommand { get; }

        public LandingViewModel()
        {
            GoToSignInCommand = new Command(GoToSignIn);
        }

        private void GoToSignIn()
        {
            Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
        }
    }

}

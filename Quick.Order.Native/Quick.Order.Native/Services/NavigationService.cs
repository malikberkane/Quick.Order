using MalikBerkane.MvvmToolkit;
using Quick.Order.AppCore.Models;
using Quick.Order.Native.ViewModels;
using Quick.Order.Native.Views;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Quick.Order.Native.Services
{
    public class NavigationService : INavigationService
    {
        private readonly ViewModelNavigationService viewModelNavigationService;

        public NavigationService(ViewModelNavigationService viewModelNavigationService)
        {
            this.viewModelNavigationService = viewModelNavigationService;
        }
        public async Task GoToMainBackOffice()
        {
            var page = new BackOfficeHomePage();

            await page.SetupBindingContext<BackOfficeHomePageModel>(page);


            var basicNavContainer = new ExtendedNavigationPage(page);
            Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(() =>
            {
                Application.Current.MainPage = basicNavContainer;

            });

        }

        public Task GoToLogin()
        {
            return viewModelNavigationService.PushPage<LoginPage, LoginViewModel>();
        }


        public async Task GoToLanding()
        {
            var page = new LandingPage();

            await page.SetupBindingContext<LandingViewModel>(page);

            var basicNavContainer = new ExtendedNavigationPage(page);
            Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(() =>
            {
                Application.Current.MainPage = basicNavContainer;

            });
        }

        public Task GoToMenuEdition(Restaurant restaurant)
        {
            return viewModelNavigationService.PushPage<MenuEditionPage, MenuEditionPageModel>(restaurant);
        }

        public Task GoToRestaurantEdition(Restaurant restaurant=null)
        {
            return viewModelNavigationService.PushPage<NewRestaurantPage, NewRestaurantPageModel>(restaurant);
        }

        public Task GoBack()
        {
            return Application.Current.MainPage.Navigation.PopAsync();
        }

        public Task GoToAddDish(Restaurant restaurant, DishSection section)
        {
            return viewModelNavigationService.PushPage<AddDishPage, AddDishViewModel>(new AddDishParams { Restaurant=restaurant,Section=section});
        }
    }

}

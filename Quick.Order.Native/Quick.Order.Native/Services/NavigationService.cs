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
        public  Task GoToMainBackOffice()
        {
            return viewModelNavigationService.CreateNavigationRoot<BackOfficeHomePage, BackOfficeHomePageModel>();

        }

        public Task GoToLogin()
        {
            return viewModelNavigationService.PushPage<LoginPage, LoginViewModel>();
        }


        public Task GoToLanding()
        {
            return viewModelNavigationService.CreateNavigationRoot<LandingPage, LandingViewModel>();

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
            return viewModelNavigationService.PushPage<AddDishPage, AddDishPageModel>(new AddDishParams { Restaurant=restaurant,Section=section});
        }

        public Task GoToAddDishSection(Restaurant restaurant)
        {
            return viewModelNavigationService.PushPage<AddDishSectionPage, AddDishSectionPageModel>(restaurant);
        }
    }

}


using MalikBerkane.MvvmToolkit;
using Quick.Order.AppCore.Authentication.Contracts;
using Quick.Order.AppCore.BusinessOperations;
using Quick.Order.AppCore.Models;
using Quick.Order.Native.Services;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Quick.Order.Native.ViewModels
{
    public class NewRestaurantPageModel : ModalPageModelBase<Restaurant>
    {
        
        private readonly BackOfficeRestaurantService restaurantService;
        private readonly IAuthenticationService authenticationService;
        private readonly IBackOfficeNavigation backOfficeNavigation;

        public NewRestaurantPageModel(BackOfficeRestaurantService restaurantService, IAuthenticationService authenticationService, IBackOfficeNavigation backOfficeNavigation)
        {
            SaveCommand = CreateAsyncCommand(OnSave);
            GoToCurrencySelectionCommand = CreateCommand(SelectCurrency);

            this.restaurantService = restaurantService;
            this.authenticationService = authenticationService;
            this.backOfficeNavigation = backOfficeNavigation;
            Currency = new Currency { Code = "USD", Symbol = "$", Name = "United States dollar" };
        }

        private bool CanSave()
        {
            return !string.IsNullOrEmpty(Text) && !string.IsNullOrEmpty(Description);
        }

        public string Text { get; set; }


        public string Description { get; set; }



        public ICommand SaveCommand { get; }
        public ICommand GoToCurrencySelectionCommand { get; }
        public Currency Currency { get; private set; }

        private async Task SelectCurrency()
        {
            var selectedCurrency = await backOfficeNavigation.GoToCurrencyChoice();

            if (selectedCurrency != null)
            {
               Currency = selectedCurrency;
            }
        }

        private async Task OnSave()
        {

            if (!CanSave())
            {
                
                return;
            }

            var newRestaurant = new Restaurant(Text, Description, AppCore.Models.Menu.CreateDefault(), authenticationService.LoggedUser.RestaurantAdmin, currency:Currency);
           

            await restaurantService.AddRestaurant(newRestaurant);

            await SetResult(newRestaurant);
        }



    }
}

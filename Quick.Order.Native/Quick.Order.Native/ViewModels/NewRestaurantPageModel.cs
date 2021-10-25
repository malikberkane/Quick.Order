
using MalikBerkane.MvvmToolkit;
using Quick.Order.AppCore.Authentication.Contracts;
using Quick.Order.AppCore.BusinessOperations;
using Quick.Order.AppCore.Models;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Quick.Order.Native.ViewModels
{
    public class NewRestaurantPageModel : ModalPageModelBase<Restaurant>
    {
        
        private readonly BackOfficeRestaurantService restaurantService;
        private readonly IAuthenticationService authenticationService;

        public NewRestaurantPageModel(BackOfficeRestaurantService restaurantService, IAuthenticationService authenticationService)
        {
            SaveCommand = CreateAsyncCommand(OnSave);
            
            this.restaurantService = restaurantService;
            this.authenticationService = authenticationService;
        }

        private bool CanSave()
        {
            return !string.IsNullOrEmpty(Text) && !string.IsNullOrEmpty(Description);
        }

        public string Text { get; set; }


        public string Description { get; set; }



        public ICommand SaveCommand { get; }

        

        private async Task OnSave()
        {

            if (!CanSave())
            {
                
                return;
            }

            var newRestaurant = new Restaurant(Text, Description, AppCore.Models.Menu.CreateDefault(), authenticationService.LoggedUser.RestaurantAdmin);
           

            await restaurantService.AddRestaurant(newRestaurant);

            await SetResult(newRestaurant);
        }



    }
}

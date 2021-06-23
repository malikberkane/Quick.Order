
using MalikBerkane.MvvmToolkit;
using Quick.Order.AppCore.BusinessOperations;
using Quick.Order.AppCore.Models;
using Quick.Order.Native.Services;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Quick.Order.Native.ViewModels
{
    public class NewRestaurantPageModel : PageModelBase
    {
        
        private readonly BackOfficeRestaurantService restaurantService;
        private readonly INavigationService navigationService;
        private readonly PageModelMessagingService messagingService;

        public NewRestaurantPageModel(BackOfficeRestaurantService restaurantService, INavigationService navigationService, PageModelMessagingService messagingService)
        {
            SaveCommand = new AsyncCommand(OnSave);
            CancelCommand = new AsyncCommand(navigationService.GoBack);
            
            this.restaurantService = restaurantService;
            this.navigationService = navigationService;
            this.messagingService = messagingService;
        }

        private bool CanSave()
        {
            return !string.IsNullOrEmpty(Text) && !string.IsNullOrEmpty(Description);
        }

        public string Text { get; set; }


        public string Description { get; set; }



        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        

        private async Task OnSave()
        {

            if (!CanSave())
            {
                return;
            }

            var newItem = new Restaurant(Text, Description, AppCore.Models.Menu.CreateDefault());
           

            await restaurantService.AddRestaurant(newItem);
            messagingService.Send("RestaurantAdded");
            await navigationService.GoBack();
        }



    }
}

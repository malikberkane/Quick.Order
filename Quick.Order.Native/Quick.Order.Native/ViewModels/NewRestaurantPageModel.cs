
using MalikBerkane.MvvmToolkit;
using Quick.Order.AppCore.BusinessOperations;
using Quick.Order.AppCore.Models;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Quick.Order.Native.ViewModels
{
    public class NewRestaurantPageModel : ModalPageModelBase<Restaurant>
    {
        
        private readonly BackOfficeRestaurantService restaurantService;


        public NewRestaurantPageModel(BackOfficeRestaurantService restaurantService)
        {
            SaveCommand = CreateAsyncCommand(OnSave);
            
            this.restaurantService = restaurantService;
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

            var newRestaurant = new Restaurant(Text, Description, AppCore.Models.Menu.CreateDefault());
           

            await restaurantService.AddRestaurant(newRestaurant);

            await SetResult(newRestaurant);
        }



    }
}

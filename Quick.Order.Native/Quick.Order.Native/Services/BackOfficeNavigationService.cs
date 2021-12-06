using Quick.Order.AppCore.Models;
using Quick.Order.Native.Popups;
using Quick.Order.Native.ViewModels;
using Quick.Order.Native.ViewModels.Modal;
using Quick.Order.Native.Views;
using System.Threading.Tasks;

namespace Quick.Order.Native.Services
{
    public class BackOfficeNavigationService: BaseNavigationService, IBackOfficeNavigation
    {
        public Task GoToMainBackOffice()
        {
            if(Xamarin.Forms.Device.Idiom== Xamarin.Forms.TargetIdiom.Desktop)
            {
                viewModelNavigationService.CreateNavigationRoot<BackOfficeHomeDesktopPage, BackOfficeHomePageModel>();

            }
            else
            {
                viewModelNavigationService.CreateNavigationRoot<BackOfficeHomePage, BackOfficeHomePageModel>();

            }
            return Task.CompletedTask;

        }

        public Task<DishEditionResult> GoToAddDish(AddDishParams addDishParams)
        {
            return viewModelNavigationService.PushModal<AddDishPopup, AddDishPageModel, DishEditionResult>(addDishParams);
        }

        public Task<DishSectionEditionOperationResult> GoToAddOrEditDishSection(EditDishSectionParams editDishSectionParams)
        {
            return viewModelNavigationService.PushModal<AddDishSectionPopup, AddDishSectionPageModel, DishSectionEditionOperationResult>(editDishSectionParams);
        }

        public Task<RestaurantIdentity> GoToEditRestaurantInfos(RestaurantIdentity restaurant)
        {
            return viewModelNavigationService.PushModal<EditRestaurantInfosPopup, EditRestaurantInfosPageModel, RestaurantIdentity>(restaurant);
        }

        public Task<DishEditionResult> GoToEditDish(EditDishParams editDishParams)
        {
            return viewModelNavigationService.PushModal<EditDishPopup, EditDishPageModel, DishEditionResult>(editDishParams);
        }

        public Task GoToOrderDetails(AppCore.Models.Order order)
        {
            return viewModelNavigationService.PushPage<OrderPage, OrdersPageModel>(order);
        }

        public Task<OrderStatusEditionResult> GoToEditOrderStatus(AppCore.Models.Order order)
        {
            return viewModelNavigationService.PushModal<EditOrderStatusPopup, EditOrderStatusPageModel, OrderStatusEditionResult>(order);
        }

        public Task GoToQrGeneration(Restaurant restaurant)
        {
            return viewModelNavigationService.PushPage<QrCodeGenerationPage, QrCodeGenerationPageModel>(restaurant);

        }

        public Task<Restaurant> GoToRestaurantEdition(Restaurant restaurant = null)
        {
            return viewModelNavigationService.PushModal<NewRestaurantPopup, NewRestaurantPageModel, Restaurant>(restaurant);
        }

        public Task<Currency> GoToCurrencyChoice()
        {
            return viewModelNavigationService.PushModal<CurrencyChoicePopup, CurrencyChoicePageModel, Currency>();
        }

        public Task GoToCultureChoice()
        {
            return viewModelNavigationService.PushModal<ChangeLanguagePopup, ChangeLanguagePageModel,object>();
        }
    }
}

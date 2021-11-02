using GoogleVisionBarCodeScanner;
using MalikBerkane.MvvmToolkit;
using Quick.Order.AppCore.BusinessOperations;
using Quick.Order.AppCore.Exceptions;
using Quick.Order.Native.Services;
using Quick.Order.Native.ViewModels.Base;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Quick.Order.Native.ViewModels
{
    public class QrCodeScanPageModel : ExtendedPageModelBase
    {

        public QrCodeScanPageModel()
        {

            GoToMenuCommand = CreateAsyncCommand<string>(GoToMenu);

        }
        public ICommand GoToMenuCommand { get; set; }

        private async Task GoToMenu(string arg)
        {
            if(Guid.TryParse(arg, out Guid restaurantId))
            {
                var restaurant = await ServicesAggregate.Repositories.Restaurants.GetById(restaurantId);

                if (restaurant == null)
                {
                    throw new RestaurantNotFoundException();
                }
                await NavigationService.Order.GoToMenu(restaurant);
            }
            else
            {
                throw new InvalidRestaurantCode();
            }

            

        }
    }


}
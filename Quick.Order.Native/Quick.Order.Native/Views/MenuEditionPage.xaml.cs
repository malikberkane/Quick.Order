using Quick.Order.Native.Services;
using System.IO;
using Xamarin.Forms;

namespace Quick.Order.Native.Views
{
    public partial class MenuEditionPage
    {
        private readonly PageModelMessagingService messagingService = FreshMvvm.FreshIOC.Container.Resolve<PageModelMessagingService>();
        public MenuEditionPage()
        {
            InitializeComponent();
            messagingService.Subscribe<Stream>("photo", (s) =>
            {
                Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(() =>
                {
                    restaurantPhoto.Source = string.Empty;
                    restaurantPhoto.Source = ImageSource.FromStream(() =>
                    {
                        return s;
                    });
                });
               
            }, this);

        }


    }
}
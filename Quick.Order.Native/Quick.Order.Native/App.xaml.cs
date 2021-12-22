using FreshMvvm;
using MalikBerkane.MvvmToolkit;
using Quick.Order.AppCore.Contracts;
using Quick.Order.Native.Services;
using System;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.Forms;
using Quick.Order.AppCore.Resources;
using Xamarin.Essentials;
using System.Threading.Tasks;

namespace Quick.Order.Native
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            MainPage = new ContentPage();
            RegisterNavigationServices();
          
            Shared.Infrastructure.Setup.Init();
            LocalizationResourceManager.Current.PropertyChanged += (sender, e) => AppResources.Culture = LocalizationResourceManager.Current.CurrentCulture;
            LocalizationResourceManager.Current.Init(AppResources.ResourceManager);
            var connectivityService = FreshIOC.Container.Resolve<IConnectivityService>();

            connectivityService.ConnectivityStateChanged += ConnectivityService_ConnectivityStateChanged;


            StartupLogic();
        }

        private static void RegisterNavigationServices()
        {
            FreshIOC.Container.Register<IAlertUserService, AlertUserService>();
            FreshIOC.Container.Register<ISignInNavigation, SignInNavigationService>();
            FreshIOC.Container.Register<IBackOfficeNavigation, BackOfficeNavigationService>();
            FreshIOC.Container.Register<ITakeOrderNavigation, OrderNavigationService>();
            FreshIOC.Container.Register<ICommonNavigation, CommonNavigationService>();
            FreshIOC.Container.Register<INavigationService, NavigationService>();
        }

        private void ConnectivityService_ConnectivityStateChanged(object sender, NetworkAccessStateChanged args)
        {
            var alertService = FreshIOC.Container.Resolve<IAlertUserService>();

            if (args.NetworkRestored)
            {
                alertService.ShowSnack(AppResources.ConnectionRestored, AlertType.Success);
            }
            else
            {
                alertService.ShowSnack(AppResources.ConnectionLost, AlertType.Error);

            }
        }

        private void StartupLogic()
        {

            SetGlobaleValues(DeviceDisplay.MainDisplayInfo.Orientation, DeviceDisplay.MainDisplayInfo.Width);
            DeviceDisplay.MainDisplayInfoChanged += DeviceDisplay_MainDisplayInfoChanged;
            var localState = FreshIOC.Container.Resolve<ILocalHistoryService>();

            var savedAppCulture = localState.GetSavedAppCulture();

            if (savedAppCulture != null)
            {
                LocalizationResourceManager.Current.CurrentCulture = savedAppCulture;

            }

            var navService = FreshIOC.Container.Resolve<INavigationService>();
            var localOrder = localState.GetLocalPendingOrder();


            //if (Device.Idiom == TargetIdiom.Desktop)
            //{
            //    navService.SignIn.GoToLogin();
            //    return;

            //}


            if (!string.IsNullOrEmpty(localOrder.id))
            {
                if (localOrder.orderDate.Date != DateTime.Now.Date)
                {
                    localState.RemoveLocalPendingOrder();
                    navService.SignIn.GoToLanding();
                }
                else
                {
                    navService.Order.GoToWaitingForOrderContext(Guid.Parse(localOrder.id));
                }
            }
            else
            {
                navService.SignIn.GoToLanding();
            }
        }

        private async void DeviceDisplay_MainDisplayInfoChanged(object sender, DisplayInfoChangedEventArgs e)
        {
            await Task.Delay(500);

            SetGlobaleValues(DeviceDisplay.MainDisplayInfo.Orientation, DeviceDisplay.MainDisplayInfo.Width);
        }

        private static void SetGlobaleValues(DisplayOrientation orientation, double width)
        {

            if(Device.Idiom== TargetIdiom.Desktop)
            {
                GlobalResources.Current.ThirdOfScreenWidth = 400;
                return;
            }
            if (orientation == DisplayOrientation.Landscape)
            {
                GlobalResources.Current.ThirdOfScreenWidth = width / 3;
                GlobalResources.Current.ListMargin = new Thickness(200, 0);
            }
            else
            {
                GlobalResources.Current.ThirdOfScreenWidth = width / 2;
                GlobalResources.Current.ListMargin = new Thickness(20, 0);

            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }


        protected override void OnAppLinkRequestReceived(Uri uri)
        {
            base.OnAppLinkRequestReceived(uri);
            FreshIOC.Container.Register<INavigationService, NavigationService>();

            Shared.Infrastructure.Setup.Init();

            var navService = FreshIOC.Container.Resolve<INavigationService>();
            var deepLinkService = FreshIOC.Container.Resolve<IDeepLinkService>();

            navService.SignIn.GoToLanding(deepLinkService.ExtractRestaurantIdFromUri(uri));

            

        }
    }
}

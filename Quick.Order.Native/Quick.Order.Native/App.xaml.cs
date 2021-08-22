using FreshMvvm;
using MalikBerkane.MvvmToolkit;
using Quick.Order.AppCore.Contracts;
using Quick.Order.Native.Services;
using System;
using Xamarin.Forms;

namespace Quick.Order.Native
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            FreshMvvm.FreshIOC.Container.Register<INavigationService, NavigationService>();
            FreshMvvm.FreshIOC.Container.Register<IAlertUserService, AlertUserService>();

            Quick.Order.Shared.Infrastructure.Setup.Init();

            var connectivityService= FreshIOC.Container.Resolve<IConnectivityService>();

            connectivityService.ConnectivityStateChanged += ConnectivityService_ConnectivityStateChanged;
            InitialNavigationLogic();
        }

        private void ConnectivityService_ConnectivityStateChanged(object sender, NetworkAccessStateChanged args)
        {
            var alertService = FreshIOC.Container.Resolve<IAlertUserService>();

            if (args.NetworkRestored)
            {
                alertService.ShowSnack("Connexion restaurée", AlertType.Success);
            }
            else
            {
                alertService.ShowSnack("Connexion perdue", AlertType.Error);

            }
        }

        private void InitialNavigationLogic()
        {
            var localState = FreshIOC.Container.Resolve<ILocalSettingsService>();
            var navService = FreshIOC.Container.Resolve<INavigationService>();
            var localOrder = localState.GetLocalPendingOrder();


            if (!string.IsNullOrEmpty(localOrder.id))
            {
                if (localOrder.orderDate.Date != DateTime.Now.Date)
                {
                    localState.RemoveLocalPendingOrder();
                    navService.GoToLanding();
                }
                else
                {
                    navService.GoToWaitingForOrderContext(Guid.Parse(localOrder.id));
                }
            }
            else
            {
                navService.GoToLanding();
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
            FreshMvvm.FreshIOC.Container.Register<INavigationService, NavigationService>();

            Quick.Order.Shared.Infrastructure.Setup.Init();

            var navService = FreshIOC.Container.Resolve<INavigationService>();
            var deepLinkService = FreshIOC.Container.Resolve<IDeepLinkService>();

            navService.GoToLanding(deepLinkService.ExtractRestaurantIdFromUri(uri));

        }
    }
}

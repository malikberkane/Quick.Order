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
            RegisterNavigationServices();

            Shared.Infrastructure.Setup.Init();

            var connectivityService = FreshIOC.Container.Resolve<IConnectivityService>();

            connectivityService.ConnectivityStateChanged += ConnectivityService_ConnectivityStateChanged;
            InitialNavigationLogic();
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
                alertService.ShowSnack("Connexion restaurée", AlertType.Success);
            }
            else
            {
                alertService.ShowSnack("Connexion perdue", AlertType.Error);

            }
        }

        private void InitialNavigationLogic()
        {
            var localState = FreshIOC.Container.Resolve<ILocalHistoryService>();
            var navService = FreshIOC.Container.Resolve<INavigationService>();
            var localOrder = localState.GetLocalPendingOrder();


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

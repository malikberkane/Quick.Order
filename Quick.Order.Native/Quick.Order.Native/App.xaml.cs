using FreshMvvm;
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
            Quick.Order.Shared.Infrastructure.Setup.Init();


            var localState = FreshIOC.Container.Resolve<ILocalSettingsService>();


           

            var navService = FreshIOC.Container.Resolve<INavigationService>();

            var localOrderId = localState.GetLocalPendingOrderId();
            if (!string.IsNullOrEmpty(localOrderId))
            {

                navService.GoToWaitingForOrderContext(Guid.Parse(localOrderId));
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
    }
}

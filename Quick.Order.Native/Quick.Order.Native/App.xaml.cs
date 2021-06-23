using FreshMvvm;
using Quick.Order.Native.Services;
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

            var navService = FreshIOC.Container.Resolve<INavigationService>();

            navService.GoToLanding();
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

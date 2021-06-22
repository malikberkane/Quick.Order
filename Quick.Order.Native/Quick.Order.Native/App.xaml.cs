using Quick.Order.Native.Services;
using Quick.Order.Native.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Quick.Order.Native
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            Quick.Order.Shared.Infrastructure.Setup.Init();
            MainPage = new NavigationPage(new LandingPage());
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

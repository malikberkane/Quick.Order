using Quick.Order.Native.ViewModels;
using Quick.Order.Native.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Quick.Order.Native
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(MenuEditionPage), typeof(MenuEditionPage));
            Routing.RegisterRoute(nameof(NewRestaurantPage), typeof(NewRestaurantPage));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//AccountPage");
        }
    }
}

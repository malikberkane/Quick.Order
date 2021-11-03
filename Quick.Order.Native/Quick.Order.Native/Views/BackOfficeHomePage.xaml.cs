using System.Globalization;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.Forms;

namespace Quick.Order.Native.Views
{
    public partial class BackOfficeHomePage : ContentPage
    {
        public BackOfficeHomePage()
        {
            InitializeComponent();
        }

        private void FrenchFlag_Tapped(object sender, System.EventArgs e)
        {
            LocalizationResourceManager.Current.CurrentCulture = new CultureInfo("fr");
        }

        private void EnFlag_Tapped(object sender, System.EventArgs e)
        {
            LocalizationResourceManager.Current.CurrentCulture = new CultureInfo("en");
        }
    }
}
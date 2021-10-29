using Quick.Order.Native.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Quick.Order.Native.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateUserPage : ContentPage
    {
        public CreateUserPage()
        {
            InitializeComponent();
            this.BindingContext = FreshMvvm.FreshIOC.Container.Resolve<CreateUserPageModel>();

        }
    }
}
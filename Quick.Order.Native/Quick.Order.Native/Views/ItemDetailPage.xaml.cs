using Quick.Order.Native.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Quick.Order.Native.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = FreshMvvm.FreshIOC.Container.Resolve<ItemDetailViewModel>();
        }
    }
}
using Quick.Order.Native.Services;
using Quick.Order.Native.ViewModels;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Quick.Order.Native.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuEditionPage
    {
        public MenuEditionPage()
        {

            InitializeComponent();

            if (Device.RuntimePlatform == Device.iOS)
            {
                menuCollectionView.SetBinding(CollectionView.ItemsSourceProperty,
                 new Binding()
                 {
                     Source = BindingContext ,
                     Path = nameof(BackOfficeHomePageModel.MenuGroupedBySectionList)

                 }); ;
            }
            else
            {
                menuCollectionView.SetBinding(CollectionView.ItemsSourceProperty,
                    new Binding()
                    {
                        Source = BindingContext,

                        Path = nameof(BackOfficeHomePageModel.MenuGroupedBySection),

                    });
            }

        }


    }
}
using Quick.Order.Native.ViewModels;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Quick.Order.Native.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QrCodeGenerationPage : ContentPage
    {

        public QrCodeGenerationPage()
        {
            InitializeComponent();

        }
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if(BindingContext is QrCodeGenerationPageModel pageModel)
            {
                QrCodeImage.Source = ImageSource.FromStream(() => new MemoryStream(pageModel.QrCodeBytes));
            }
        }
    }
}
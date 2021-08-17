using QRCoder;
using Quick.Order.AppCore.Contracts;
using System;
using System.IO;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Quick.Order.Native.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QrCodeGenerationPage : ContentPage
    {
        private byte[] qrCodeBytes;

        public QrCodeGenerationPage(string input)
        {
            InitializeComponent();

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(input, QRCodeGenerator.ECCLevel.Q);
            PngByteQRCode qRCode = new PngByteQRCode(qrCodeData);
            qrCodeBytes = qRCode.GetGraphic(20);
            QrCodeImage.Source = ImageSource.FromStream(() => new MemoryStream(qrCodeBytes));
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            var printService = FreshMvvm.FreshIOC.Container.Resolve<IPrintService>();

            printService.Print(qrCodeBytes);
        }
    }
}
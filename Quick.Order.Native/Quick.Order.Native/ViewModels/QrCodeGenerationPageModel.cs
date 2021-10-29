using Quick.Order.AppCore.Models;
using Quick.Order.Native.ViewModels.Base;
using System.Windows.Input;

namespace Quick.Order.Native.ViewModels
{
    public class QrCodeGenerationPageModel : ExtendedPageModelBase<Restaurant>
    {
        public ICommand PrintQrCodeCommand { get; private set; }

        public byte[] QrCodeBytes { get; set; }
        public QrCodeGenerationPageModel()
        {
            PrintQrCodeCommand = CreateCommand(PrintQrCode);

        }
        protected override void PostParamInitialization()
        {
            QrCodeBytes = ServicesAggregate.Business.QrCodeGeneration.CreateQrCodeBitmap(Parameter.Id.ToString());
        }
        private void PrintQrCode()
        {
            ServicesAggregate.Plugin.Printer.Print(QrCodeBytes);
        }
    }

}

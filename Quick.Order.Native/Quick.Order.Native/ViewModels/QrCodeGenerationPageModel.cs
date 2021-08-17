using Quick.Order.AppCore.BusinessOperations;
using Quick.Order.AppCore.Contracts;
using Quick.Order.AppCore.Models;
using Quick.Order.Native.ViewModels.Base;
using System.Windows.Input;

namespace Quick.Order.Native.ViewModels
{
    public class QrCodeGenerationPageModel : ExtendedPageModelBase<Restaurant>
    {
        public ICommand PrintQrCodeCommand { get; private set; }

        private readonly IPrintService printService;
        private readonly QrCodeGenerationService qrCodeGenerationService;

        public byte[] QrCodeBytes { get; set; }
        public QrCodeGenerationPageModel(IPrintService printService, QrCodeGenerationService qrCodeGenerationService)
        {
            PrintQrCodeCommand = CreateCommand(PrintQrCode);
            this.printService = printService;
            this.qrCodeGenerationService = qrCodeGenerationService;
        }

        protected override void PostParamInitialization()
        {
            QrCodeBytes = qrCodeGenerationService.CreateQrCodeBitmap(Parameter.Id.ToString());
        }
        private void PrintQrCode()
        {
            printService.Print(QrCodeBytes);
        }
    }

}

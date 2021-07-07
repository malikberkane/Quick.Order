using MalikBerkane.MvvmToolkit;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Quick.Order.Native.ViewModels
{
    public class QrCodeModalScanPageModel: ModalPageModelBase<string>
    {
        public ICommand QrCodeDetectedCommand { get; set; }

        public QrCodeModalScanPageModel()
        {
            QrCodeDetectedCommand = CreateAsyncCommand<string>(QrCodeDetected);
        }

        private Task QrCodeDetected(string qrCode)
        {
            return SetResult(qrCode);
        }
    }
}   


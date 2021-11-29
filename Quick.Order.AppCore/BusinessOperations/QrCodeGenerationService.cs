using QRCoder;
using Quick.Order.AppCore.Contracts;

namespace Quick.Order.AppCore.BusinessOperations
{
    public class QrCodeGenerationService
    {
        private readonly IDeepLinkService deepLinkService;

        public QrCodeGenerationService(IDeepLinkService deepLinkService)
        {
            this.deepLinkService = deepLinkService;
        }
        public byte[] CreateQrCodeBitmap(string restaurantId)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(deepLinkService.CreateDeepLinkUrl(restaurantId), QRCodeGenerator.ECCLevel.Q);
            PngByteQRCode qRCode = new PngByteQRCode(qrCodeData);
            return qRCode.GetGraphic(20);
        }
    }

}

using QRCoder;
namespace Quick.Order.AppCore.BusinessOperations
{
    public class QrCodeGenerationService
    {
        public byte[] CreateQrCodeBitmap(string stringToEncode)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(stringToEncode, QRCodeGenerator.ECCLevel.Q);
            PngByteQRCode qRCode = new PngByteQRCode(qrCodeData);
            return qRCode.GetGraphic(20);
        }
    }
}

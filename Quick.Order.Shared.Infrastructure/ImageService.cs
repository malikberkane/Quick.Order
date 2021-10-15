using Firebase.Storage;
using Quick.Order.AppCore.Contracts;
using System.IO;
using System.Threading.Tasks;

namespace Quick.Order.Shared.Infrastructure
{
    public class ImageService : IImageService
    {
        public async Task<string> SaveImage(string imageName, Stream imageStream)
        {
            var storagrUrl = await new FirebaseStorage("quickorder-f339b.appspot.com")
                .Child($"{imageName}.jpg")
                .PutAsync(imageStream);
            return storagrUrl;
        }
    }

}

using System.IO;
using System.Threading.Tasks;

namespace Quick.Order.AppCore.Contracts
{
    public interface IImageService
    {
        Task<string> SaveImage(string imageName, Stream imageStream);
    }
}

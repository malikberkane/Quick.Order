using System.Threading.Tasks;

namespace Quick.Order.Native.Services
{
    public interface ISignInNavigation
    {
        Task GoToLanding(string scannedCode = null);
        Task GoToLogin();
        Task GoToCreateUser();

    }
}
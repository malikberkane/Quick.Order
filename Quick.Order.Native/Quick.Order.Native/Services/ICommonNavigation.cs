using System.Threading.Tasks;

namespace Quick.Order.Native.Services
{
    public interface ICommonNavigation
    {
        Task GoBack();
        Task<bool> PromptForConfirmation(string message, string confirm, string cancel = null, bool isDestructive = true);

    }
}
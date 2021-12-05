using Quick.Order.Native.Popups;
using Quick.Order.Native.ViewModels;
using System.Threading.Tasks;

namespace Quick.Order.Native.Services
{
    public class CommonNavigationService : BaseNavigationService, ICommonNavigation
    {
        public async Task<bool> PromptForConfirmation(string message, string confirm, string cancel = null, bool isDestructive=true)
        {
            var confirmationPromptParams = new ConfirmationParams { CancelLabel = cancel, OkLabel = confirm, Message = message, IsDestructive=isDestructive };
            var result = await viewModelNavigationService.PushModal<ConfirmationPopup, ConfirmationPageModel, ConfirmationResult>(confirmationPromptParams);

            return result != null && result.IsConfirmed;
        }

        public Task GoBack()
        {
            return viewModelNavigationService.Pop();
        }

    }
}

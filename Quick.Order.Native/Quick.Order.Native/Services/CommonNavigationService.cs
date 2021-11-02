using Quick.Order.Native.Popups;
using Quick.Order.Native.ViewModels;
using System.Threading.Tasks;

namespace Quick.Order.Native.Services
{
    public class CommonNavigationService : BaseNavigationService, ICommonNavigation
    {
        public async Task<bool> PromptForConfirmation(string title, string message, string confirm, string cancel = null)
        {
            var confirmationPromptParams = new ConfirmationParams { CancelLabel = cancel, OkLabel = confirm, Message = message };
            var result = await viewModelNavigationService.PushModal<ConfirmationPopup, ConfirmationPageModel, ConfirmationResult>(confirmationPromptParams);

            return result != null && result.IsConfirmed;
        }

        public Task GoBack()
        {
            return viewModelNavigationService.Pop();
        }

    }
}

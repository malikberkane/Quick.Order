using MalikBerkane.MvvmToolkit;
using Quick.Order.AppCore.Contracts;
using Quick.Order.Native.ViewModels.Base;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Helpers;

namespace Quick.Order.Native.ViewModels.Modal
{
    public class ChangeLanguagePageModel: ModalPageModelBase<object,object>
    {
        private readonly ILocalHistoryService localHistoryService;

        public List<CultureInfo> SupportedCultures { get; set; } = new List<CultureInfo>() { new CultureInfo("fr"), new CultureInfo("en") };
    
        public ChangeLanguagePageModel(ILocalHistoryService localHistoryService)
        {
            SelectCultureCommand = CreateAsyncCommand<CultureInfo>(SelectCulture);
            this.localHistoryService = localHistoryService;
        }

        public ICommand SelectCultureCommand { get; }

        private Task SelectCulture(CultureInfo selectedCulture)
        {
            LocalizationResourceManager.Current.CurrentCulture = selectedCulture;
            localHistoryService.SetAppCulture(selectedCulture);
            return SetResult();
        }
    }




}
using MalikBerkane.MvvmToolkit;
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

        public List<CultureInfo> SupportedCultures { get; set; } = new List<CultureInfo>() { new CultureInfo("fr"), new CultureInfo("en") };
    
        public ChangeLanguagePageModel()
        {
            SelectCultureCommand = CreateAsyncCommand<CultureInfo>(SelectCulture);
        }

        public ICommand SelectCultureCommand { get; }

        private Task SelectCulture(CultureInfo arg)
        {
            LocalizationResourceManager.Current.CurrentCulture = arg;
            return SetResult();
        }
    }




}
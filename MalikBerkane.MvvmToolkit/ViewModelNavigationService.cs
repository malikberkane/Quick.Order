using FreshMvvm;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MalikBerkane.MvvmToolkit
{
    public class ViewModelNavigationService
    {

        public async Task PushPage<TPage, TPageModel>(object param = null) where TPage : ContentPage where TPageModel : IPageModel
        {
            var page = FreshIOC.Container.Resolve<TPage>();
            var bindingContext = page.SetupBindingContext<TPageModel>(param);

            await Device.InvokeOnMainThreadAsync(async () =>
            {
                await (Application.Current.MainPage as ExtendedNavigationPage).PushAsync(page);
            });


        }

        public Task CloseModal()
        {
            return Application.Current.MainPage.Navigation.PopPopupAsync();

        }

        public async Task<TModalResult> PushModal<TPage, TPageModel, TModalResult>(object param = null)
           where TPage : PopupPage
           where TPageModel : IModalPageModel<TModalResult>
           where TModalResult : class
        {


            var modal = FreshIOC.Container.Resolve<TPage>();
            var bindingContext =  modal.SetupBindingContext<TPageModel>(param);
            await Device.InvokeOnMainThreadAsync(async () =>
            {
                await Application.Current.MainPage.Navigation.PushPopupAsync(modal);

            });
            return await bindingContext.Result.Task;

        }

        public void CreateNavigationRoot<TPage, TPageModel>(object param = null) where TPage : ContentPage where TPageModel : IPageModel
        {
            var page = FreshIOC.Container.Resolve<TPage>();
            var bindingContext = page.SetupBindingContext<TPageModel>(param);

            var basicNavContainer = new ExtendedNavigationPage(page);
            Device.BeginInvokeOnMainThread(() =>
            {
                
               Application.Current.MainPage = basicNavContainer;

               
            });
           

        }



    }


    public static class NavigationExtensions
    {
        public static  T SetupBindingContext<T>(this Page targetPage, object data = null) where T : IPageModel
        {
            var pageModel = FreshIOC.Container.Resolve(typeof(T)) as IPageModel;
            if (pageModel is T typedPageModel)
            {
                pageModel.Init(data);

                targetPage.BindingContext = pageModel;
                targetPage.Appearing += pageModel.OnAppearing;
                targetPage.Disappearing += pageModel.OnDisappearing;
                return typedPageModel;
            }
            else
            {
                throw new System.Exception("Impossible to cast to right pageModel type");
            }


        }
    }


}

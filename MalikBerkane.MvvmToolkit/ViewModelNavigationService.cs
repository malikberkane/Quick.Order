using FreshMvvm;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.CommunityToolkit.Extensions;

namespace MalikBerkane.MvvmToolkit
{
    public class ViewModelNavigationService
    {
        private static Xamarin.CommunityToolkit.UI.Views.Popup currentModal;

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
            currentModal?.Dismiss(null);

            return Task.CompletedTask;
        }

        public Task Pop()
        {

            return Device.InvokeOnMainThreadAsync(async () =>
            {
                await (Application.Current.MainPage as ExtendedNavigationPage).PopAsync();
            });

        }


        public async Task<TModalResult> PushModal<TPage, TPageModel, TModalResult>(object param = null)
           where TPage : Xamarin.CommunityToolkit.UI.Views.Popup
           where TPageModel : IModalPageModel<TModalResult>
           where TModalResult : class
        {


            currentModal = FreshIOC.Container.Resolve<TPage>();

            var bindingContext =  currentModal.SetupBindingContext<TPageModel>(param);
            await Device.InvokeOnMainThreadAsync(() =>
            {
                Application.Current.MainPage.Navigation.ShowPopup(currentModal);

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
        public static T SetupBindingContext<T>(this Page targetPage, object data = null) where T : IPageModel
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
        public static  T SetupBindingContext<T>(this Xamarin.CommunityToolkit.UI.Views.Popup targetPage, object data = null) where T : IPageModel
        {
            var pageModel = FreshIOC.Container.Resolve(typeof(T)) as IPageModel;
            if (pageModel is T typedPageModel)
            {
                pageModel.Init(data);

                targetPage.BindingContext = pageModel;

                return typedPageModel;
            }
            else
            {
                throw new System.Exception("Impossible to cast to right pageModel type");
            }


        }
    }


}

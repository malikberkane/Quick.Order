﻿using FreshMvvm;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MalikBerkane.MvvmToolkit
{
    public class ViewModelNavigationService
    {

        public async Task PushPage<TPage, TPageModel>(object param = null) where TPage : ContentPage where TPageModel : IPageModel
        {
            var page = FreshIOC.Container.Resolve<TPage>();
            var bindingContext = await page.SetupBindingContext<TPageModel>(param);

            await Device.InvokeOnMainThreadAsync(async () =>
            {
                await (Application.Current.MainPage as ExtendedNavigationPage).PushAsync(page);
            });


        }


       
    }


    public static class NavigationExtensions
    {
        public static async Task<T> SetupBindingContext<T>(this Page targetPage, object data = null) where T : IPageModel
        {
            var pageModel = FreshIOC.Container.Resolve(typeof(T)) as IPageModel;
            if (pageModel is T typedPageModel)
            {
                await pageModel.Init(data);
                
                targetPage.BindingContext = pageModel;
                targetPage.Appearing += pageModel.OnAppearing;
                return typedPageModel;
            }
            else
            {
                throw new System.Exception("Impossible to cast to right pageModel type");
            }


        }
    }


    public class ExtendedNavigationPage: NavigationPage
    {
        public ExtendedNavigationPage(Page root) : base(root)
        {
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Popped += this.OnPopped;
        }


        private void OnPopped(object sender, NavigationEventArgs e)
        {
            if (e.Page.BindingContext is IPageModel pageModel)
            {
                try
                {
                    pageModel.CleanUp();
                    e.Page.Appearing -= pageModel.OnAppearing;
                    e.Page.BindingContext = null;
                }
                catch (System.Exception)
                {

                    //Exception sur une page qui n'existe plus.
                }
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Popped -= this.OnPopped;
        }

    }


}

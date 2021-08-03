using MalikBerkane.MvvmToolkit;
using System;
using Xamarin.Forms;

namespace Quick.Order.Native.ViewModels.Base
{
    public class ExtendedPageModelBase<TParameter> : PageModelBase<TParameter> where TParameter : class
    {
        protected override void OnExceptionCaught(Exception ex)
        {
            HandleError(ex.Message);
        }

        protected void HandleError(string errorMessage)
        {
            Application.Current.MainPage.DisplayAlert("Error", errorMessage, "ok");

        }
    }

    public abstract class ExtendedPageModelBase : ExtendedPageModelBase<object>
    {
        
    }
}

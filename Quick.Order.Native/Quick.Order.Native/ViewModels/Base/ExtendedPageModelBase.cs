using FreshMvvm;
using MalikBerkane.MvvmToolkit;
using Quick.Order.AppCore.Resources;
using Quick.Order.AppCore.ServicesAggregate;
using Quick.Order.Native.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Quick.Order.Native.ViewModels.Base
{
    public class ExtendedPageModelBase<TParameter> : PageModelBase<TParameter> where TParameter : class
    {

        public ICommand GoBackCommand { get; set; }

        public ExtendedPageModelBase()
        {
            GoBackCommand = CreateCommand(NavigationService.Common.GoBack);

        }
        protected ServicesAggregate ServicesAggregate { get; } = FreshIOC.Container.Resolve<ServicesAggregate>();
        protected IAlertUserService AlertUserService { get; } = FreshIOC.Container.Resolve<IAlertUserService>();

        protected INavigationService NavigationService { get; } = FreshIOC.Container.Resolve<INavigationService>();

        protected PageModelMessagingService MessagingService { get; } = FreshIOC.Container.Resolve<PageModelMessagingService>();

        protected override void OnExceptionCaught(Exception ex)
        {
            ServicesAggregate.Plugin.Logger.Log(ex);
            HandleError(ex.Message);
        }



        protected void HandleError(string errorMessage)
        {
            AlertUserService.ShowSnack(errorMessage, AlertType.Error);

        }


        public Task DisplayAlert(string title, string message)
        {
            return Application.Current.MainPage.DisplayAlert(title, message, "ok");

        }




        public override  ICommand CreateAsyncCommand<T>(Func<T, Task> action, Func<T, bool> canExecute = null, IErrorHandler errorHandler = null, bool setPageModelToLoadingState = true)
        {
           
            return base.CreateAsyncCommand(action, AddConnectivityCheckToCanExectute(canExecute), errorHandler, setPageModelToLoadingState);

        }

        public override ICommand CreateAsyncCommand(Func<Task> action, Func<bool> canExecute = null, IErrorHandler errorHandler = null)
        {
            return base.CreateAsyncCommand(action, AddConnectivityCheckToCanExectute(canExecute), errorHandler);
        }

        public  ICommand CreateOfflineAsyncCommand(Func<Task> action, Func<bool> canExecute = null, IErrorHandler errorHandler = null)
        {
            return base.CreateAsyncCommand(action, canExecute, errorHandler);
        }

        public override ICommand CreateCommand(Action action, Func<bool> canExecute = null, IErrorHandler errorHandler = null)
        {
            return base.CreateCommand(action, AddConnectivityCheckToCanExectute(canExecute), errorHandler);
        }

        public override ICommand CreateCommand(Func<Task> action, Func<bool> canExecute = null, IErrorHandler errorHandler = null)
        {
            return base.CreateCommand(action, AddConnectivityCheckToCanExectute(canExecute), errorHandler);
        }

        public ICommand CreateOfflineCommand(Func<Task> action, Func<bool> canExecute = null, IErrorHandler errorHandler = null)
        {
            return base.CreateCommand(action, canExecute, errorHandler);
        }

        public override ICommand CreateCommand<T>(Func<T, Task> action, Func<T, bool> canExecute = null, IErrorHandler errorHandler = null)
        {
            return base.CreateCommand(action, AddConnectivityCheckToCanExectute(canExecute), errorHandler);
        }
       
        public Func<T, bool> AddConnectivityCheckToCanExectute<T>(Func<T, bool> canExecute)
        {
            return
               (t) =>
               {
                   if (!ServicesAggregate.Plugin.Connectivity.HasNetwork())
                   {
                       HandleError(AppResources.RestoreInternetConnectionAlert);
                       return false;
                   }
                   return canExecute?.Invoke(t) ?? true;
               };
        }


        public Func<bool> AddConnectivityCheckToCanExectute(Func<bool> canExecute)
        {
            return
               () =>
               {
                   if (!ServicesAggregate.Plugin.Connectivity.HasNetwork())
                   {
                       HandleError(AppResources.RestoreInternetConnectionAlert);
                       return false;
                   }
                   return canExecute?.Invoke() ?? true;
               };
        }


    }

    public abstract class ExtendedPageModelBase : ExtendedPageModelBase<object>
    {

    }

}

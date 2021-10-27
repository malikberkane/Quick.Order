using FreshMvvm;
using MalikBerkane.MvvmToolkit;
using Quick.Order.AppCore.Contracts;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Quick.Order.Native.ViewModels.Base
{
    public class ExtendedPageModelBase<TParameter> : PageModelBase<TParameter> where TParameter : class
    {
        
       
       
        protected ILoggerService LoggerService = FreshIOC.Container.Resolve<ILoggerService>();
        protected IConnectivityService ConnectivityService = FreshIOC.Container.Resolve<IConnectivityService>();
        protected IAlertUserService AlertUserService = FreshIOC.Container.Resolve<AlertUserService>();

        protected override void OnExceptionCaught(Exception ex)
        {
            LoggerService.Log(ex);
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




        public override ICommand CreateAsyncCommand<T>(Func<T, Task> action, Func<T, bool> canExecute = null, IErrorHandler errorHandler = null, bool setPageModelToLoadingState = true)
        {
            return base.CreateAsyncCommand(action, AddConnectivityCheckToCanExectute(canExecute), errorHandler, setPageModelToLoadingState);
        }

        public override ICommand CreateAsyncCommand(Func<Task> action, Func<bool> canExecute = null, IErrorHandler errorHandler = null)
        {
            return base.CreateAsyncCommand(action, AddConnectivityCheckToCanExectute(canExecute), errorHandler);
        }


        public override ICommand CreateCommand(Action action, Func<bool> canExecute = null, IErrorHandler errorHandler = null)
        {
            return base.CreateCommand(action, AddConnectivityCheckToCanExectute(canExecute), errorHandler);
        }

        public override ICommand CreateCommand(Func<Task> action, Func<bool> canExecute = null, IErrorHandler errorHandler = null)
        {
            return base.CreateCommand(action, AddConnectivityCheckToCanExectute(canExecute), errorHandler);
        }

        public override ICommand CreateCommand<T>(Func<T, Task> action, Func<T, bool> canExecute = null, IErrorHandler errorHandler = null)
        {
            return base.CreateCommand(action, AddConnectivityCheckToCanExectute(canExecute), errorHandler);
        }
        public Func<T, bool> AddConnectivityCheckToCanExectute<T>(Func<T, bool> canExecute)
        {
            return
               (T) =>
               {
                   if (!ConnectivityService.HasNetwork())
                   {
                       HandleError("Veuillez retrouver la connection");
                       return false;
                   }
                   return canExecute?.Invoke(T) ?? true;
               };
        }


        public Func<bool> AddConnectivityCheckToCanExectute(Func<bool> canExecute)
        {
            return
               () =>
               {
                   if (!ConnectivityService.HasNetwork())
                   {
                       HandleError("Veuillez retrouver la connection");
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

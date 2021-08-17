
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Xamarin.Forms;

namespace MalikBerkane.MvvmToolkit
{

    public class PageModelBase<TParameter> : ObservableObject, IPageModel 
    {
        public bool IsLoaded { get; private set; }

        public bool IsLoading { get; set; }

        public TParameter Parameter { get; set; }


        public virtual string ContextTitle => string.Empty;

        public bool CanGoBack { get; set; }

        public void Init(object initData)
        {
            if (initData != null && !(initData is TParameter))
            {
                throw new Exception("Wrong argument");
            }

            Parameter =(TParameter) initData;

            

            PostParamInitialization();
        }

        protected virtual void PostParamInitialization()
        {

        }


        public virtual Task InitAsync()
        {
            return Task.CompletedTask;
        }




        public async Task EnsurePageModelIsInLoadingState(Func<Task> action, bool delay = false)
        {
            if (IsLoading)
            {
                return;
            }
            try
            {
                IsLoading = true;
                OnPropertyChanged(nameof(IsLoading));
                if (delay)
                {
                    await Task.Delay(100);
                }
                await action();
            }
            catch (Exception ex)
            {
                OnExceptionCaught(ex);
            }
            finally
            {
                IsLoading = false;
                OnPropertyChanged(nameof(IsLoading));

            }
        }

        protected virtual void OnExceptionCaught(Exception ex)
        {
        }


        public ICommand CreateAsyncCommand(Func<Task> action, Func<bool> canExecute = null, IErrorHandler errorHandler = null)
        {
            return new AsyncCommand(action, canExecute, errorHandler, this, true);
        }

        public ICommand CreateAsyncCommand<Param>(Func<Param, Task> action, Param parameter, Func<bool> canExecute = null, IErrorHandler errorHandler = null) where Param : class
        {
            return new AsyncCommand(async () => { await action(parameter); }, canExecute, errorHandler, this, true);
        }

        public ICommand CreateCommand(Func<Task> action, Func<bool> canExecute = null, IErrorHandler errorHandler = null)
        {
            return new AsyncCommand(action, canExecute, errorHandler, this, false);
        }

        public ICommand CreateCommand(Action action, Func<bool> canExecute = null, IErrorHandler errorHandler = null)
        {
            if (canExecute != null)
            {
                return new Command(action, canExecute);

            }
            else
            {
                return new Command(action);

            }
        }

        public ICommand CreateCommand<T>(Func<T,Task> action, Func<T,bool> canExecute = null, IErrorHandler errorHandler = null)
        {
            return new AsyncCommand<T>(action, canExecute, errorHandler, this, false);
        }

        public ICommand CreateAsyncCommand<T>(Func<T, Task> action, Func<T, bool> canExecute = null, IErrorHandler errorHandler = null, bool setPageModelToLoadingState = true)
        {
            return new AsyncCommand<T>(action, canExecute, errorHandler, this, true);
        }
        public async Task EnsurePageModelIsInLoadingState<T>(Func<T, Task> action, T param, bool delay = false) where T : class
        {
            if (IsLoading)
            {
                return;
            }
            try
            {
                IsLoading = true;

                if (delay)
                {
                    await Task.Delay(100);
                }
                await action(param);
            }
            catch (Exception ex)
            {
                OnExceptionCaught(ex);

            }
            finally
            {
                IsLoading = false;
            }
        }


        protected virtual Task Refresh()
        {
            return Task.CompletedTask;
        }

        public virtual Task CleanUp()
        {
            return Task.CompletedTask;
        }

        public async void OnAppearing(object sender, EventArgs e)
        {
            if (IsLoaded)
            {
                await EnsurePageModelIsInLoadingState(Refresh);

            }
            else
            {

                await EnsurePageModelIsInLoadingState(InitAsync);
                IsLoaded = true;

            }
        }
    }


    public abstract class PageModelBase : PageModelBase<object>
    {

    }

}

using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace MalikBerkane.MvvmToolkit
{

    public class PageModelBase<TParameter> : ObservableObject, IPageModel
    {
        public bool IsLoaded { get; private set; }

        public bool IsLoading { get; set; }

        public TParameter Parameter { get; set; }


        public virtual string ContextTitle => string.Empty;

        public bool CanGoBack { get; set; }

        public virtual void Init(object initData)
        {
            if (initData != null && !(initData is TParameter))
            {
                throw new Exception("Wrong argument");
            }

            Parameter = (TParameter)initData;



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


        public virtual ICommand CreateAsyncCommand(Func<Task> action, Func<bool> canExecute = null, IErrorHandler errorHandler = null)
        {
            return new AsyncCommand(action, canExecute, errorHandler, this, true);
        }

        public virtual ICommand CreateAsyncCommand<T>(Func<T, Task> action, Func<T, bool> canExecute = null, IErrorHandler errorHandler = null, bool setPageModelToLoadingState = true)
        {
            return new AsyncCommand<T>(action, canExecute, errorHandler, this, true);
        }


        public virtual ICommand CreateCommand(Func<Task> action, Func<bool> canExecute = null, IErrorHandler errorHandler = null)
        {
            return new AsyncCommand(action, canExecute, errorHandler, this, false);
        }


        public virtual ICommand CreateCommand<T>(Func<T, Task> action, Func<T, bool> canExecute = null, IErrorHandler errorHandler = null)
        {
            return new AsyncCommand<T>(action, canExecute, errorHandler, this, false);
        }

        public virtual ICommand CreateCommand(Action action, Func<bool> canExecute = null, IErrorHandler errorHandler = null)
        {
            Func<Task> operation = () => { action.Invoke(); return Task.CompletedTask; };

            return CreateCommand(operation, canExecute, errorHandler);
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

        public virtual void OnDisappearing(object sender, EventArgs e)
        {

        }
    }


    public abstract class PageModelBase : PageModelBase<object>
    {

    }

}

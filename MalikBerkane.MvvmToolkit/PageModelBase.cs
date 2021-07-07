
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Xamarin.Forms;

namespace MalikBerkane.MvvmToolkit
{


    public class PageModelBase<TParameter> : ObservableObject,IPageModel  where TParameter:class
    {
        private bool _isLoaded;

        public bool IsLoading { get; set; }

        public TParameter Parameter { get; set; }


        public virtual string ContextTitle => string.Empty;

        public void Init(object initData)
        {
            if(initData!= null && !(initData is TParameter))
            {
                throw new Exception("Wrong argument");
            }

            Parameter = initData as TParameter ;

            PostParamInitialization();
        }

        protected void PostParamInitialization()
        {
            
        }


        public virtual Task InitAsync()
        {
            return Task.CompletedTask;
        }


        

        public async Task EnsurePageModelIsInLoadingState(Func<Task> action,bool delay=false)
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
             Application.Current.MainPage.DisplayAlert("Exception", ex.Message, "ok");
        }


        public ICommand CreateAsyncCommand(Func<Task> action, Func<bool> canExecute=null,IErrorHandler errorHandler=null, bool setPageModelToLoadingState=true)
        {
            return new AsyncCommand(action, canExecute, errorHandler, this, setPageModelToLoadingState);
        }

        public ICommand CreateAsyncCommand<T>(Func<T,Task> action, Func<T,bool> canExecute = null, IErrorHandler errorHandler = null, bool setPageModelToLoadingState = true)
        {
            return new AsyncCommand<T>(action, canExecute, errorHandler, this, setPageModelToLoadingState);
        }
        public async Task EnsurePageModelIsInLoadingState<T>(Func<T,Task> action, T param, bool delay = false) where T :class
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
            if (_isLoaded)
            {
                await EnsurePageModelIsInLoadingState(Refresh);

            }
            else
            {
                await EnsurePageModelIsInLoadingState(InitAsync);
                _isLoaded = true;
            }
        }
    }


    public abstract class PageModelBase : PageModelBase<object>
    {
      
    }

}

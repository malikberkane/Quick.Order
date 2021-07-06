    
using System;
using System.Threading.Tasks;
using Microsoft.Toolkit.Mvvm.ComponentModel;


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
            throw ex;
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


    public interface IPageModel
    {
        void Init(object initData);
        Task CleanUp();
        void OnAppearing(object sender, EventArgs e);
    }

}

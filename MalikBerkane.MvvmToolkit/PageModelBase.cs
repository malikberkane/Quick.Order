    
using System;
using System.Threading.Tasks;
using Microsoft.Toolkit.Mvvm.ComponentModel;


namespace MalikBerkane.MvvmToolkit
{


    public class PageModelBase<TParameter> : ObservableObject,IPageModel  where TParameter:class
    {
        private bool _isLoaded;
        private int appearances;

        public bool IsLoading { get; set; }

        public TParameter Parameter { get; set; }


        public virtual string ContextTitle => string.Empty;

        public async Task Init(object initData)
        {
            if(initData!= null && !(initData is TParameter))
            {
                throw new Exception("Wrong argument");
            }

            Parameter = initData as TParameter ;

            await  EnsurePageModelIsInLoadingState(async () => await InitAsync());

            _isLoaded = true;
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

        public void OnAppearing(object sender, EventArgs e)
        {
            if (_isLoaded && appearances>0)
            {
                Refresh();
            }
            appearances++;
        }
    }


    public abstract class PageModelBase : PageModelBase<object>
    {
      
    }


    public interface IPageModel
    {
        Task Init(object initData);
        Task CleanUp();
        void OnAppearing(object sender, EventArgs e);
    }

}

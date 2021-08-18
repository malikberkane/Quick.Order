using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MalikBerkane.MvvmToolkit
{
    public class ModalPageModelBase<TParameter, TResult> : PageModelBase<TParameter>, IModalPageModel<TResult> where TParameter : class where TResult : class
    {

        public TaskCompletionSource<TResult> Result { get; set; } = new TaskCompletionSource<TResult>();

        public ICommand PopCommand { get; set; }
        public ModalPageModelBase()
        {
            PopCommand = new AsyncCommand(CancelModalTask);
        }


        private bool isSettingResult;
        protected ViewModelNavigationService ViewModelNavigationService { get; set; } = FreshMvvm.FreshIOC.Container.Resolve<ViewModelNavigationService>();

        protected virtual bool CompleteTaskBeforeClosingModal => false;
        protected Task SetResult(TResult result=null)
        {
            if (isSettingResult || Result.Task.IsCompleted)
            {
                return Task.CompletedTask ;
            }
            try
            {
                isSettingResult = true;
                if (CompleteTaskBeforeClosingModal)
                {
                    Result.SetResult(result);
                    ViewModelNavigationService.CloseModal();

                }
                else
                {
                    ViewModelNavigationService.CloseModal();
                    Result.SetResult(result);
                }
                return Task.CompletedTask;

            }
            finally
            {
                isSettingResult = false;

            }
        }

        public Task CancelModalTask()
        {
            if (IsLoading)
            {
                return Task.CompletedTask;
            }
            return SetResult(null);
        }

        protected override void OnExceptionCaught(Exception ex)
        {
            Application.Current.MainPage.DisplayAlert("Error", ex.Message, "ok");
        }


    }


    public class ModalPageModelBase<TResult>: ModalPageModelBase<object, TResult> where TResult:class
    {

    }
    public interface IModalPageModel<TResult> : ICancelableModal,IPageModel where TResult : class
    {
       TaskCompletionSource<TResult> Result { get; set; }
       ICommand PopCommand { get; set; }
    }

    public interface ICancelableModal
    {
        Task CancelModalTask();
       
    }

}

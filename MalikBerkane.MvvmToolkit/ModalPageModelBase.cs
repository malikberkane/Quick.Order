using System.Threading.Tasks;
using System.Windows.Input;

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
                    //await CoreServices.Navigation.CloseModal();

                }
                else
                {
                    //await CoreServices.Navigation.CloseModal();
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
            return SetResult(null);
        }




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

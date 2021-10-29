using MalikBerkane.MvvmToolkit;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Quick.Order.Native.ViewModels
{
    public class ConfirmationPageModel : ModalPageModelBase<ConfirmationParams, ConfirmationResult>
    {
        public ICommand ConfirmCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public string Message { get; set; }

        public string OkLabel { get; set; }

        public string CancelLabel { get; set; }
        public ConfirmationPageModel()
        {
            ConfirmCommand = CreateAsyncCommand(Confirm);
            CancelCommand = CreateAsyncCommand(Cancel);
        }

        private Task Cancel()
        {
            return SetResult(new ConfirmationResult { IsConfirmed = false });
        }

        private Task Confirm()
        {
            return SetResult(new ConfirmationResult { IsConfirmed = true });
        }

        protected override void PostParamInitialization()
        {
            if (Parameter == null)
            {
                throw new Exception("Confirmation prompt param cannot be null");
            }
            Message = Parameter.Message;
            OkLabel = Parameter.OkLabel;
            CancelLabel = Parameter.CancelLabel;
        }
    }

    public class ConfirmationParams
    {
        public string OkLabel { get; set; }
        public string CancelLabel { get; set; }

        public string Message { get; set; }

        
    }

    public class ConfirmationResult
    {
        public bool IsConfirmed { get; set; }
    }
}


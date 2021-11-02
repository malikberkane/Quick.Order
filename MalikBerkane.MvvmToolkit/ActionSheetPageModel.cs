using System.Collections.Generic;
using System.Windows.Input;

namespace MalikBerkane.MvvmToolkit
{
    public class ActionSheetPageModel : ModalPageModelBase<ActionSheetParams, string>
    {
        public ICommand SelectActionCommand { get; set; }


        public ActionSheetPageModel()
        {
            SelectActionCommand = new AsyncCommand<string>(async (t) => { await SetResult(t); });
        }



        public IEnumerable<string> Actions { get; set; }

        public override string ContextTitle => Parameter?.ModalTitle;


        protected override void PostParamInitialization()
        {
            Actions = Parameter.Options;
        }



    }

    public class ActionSheetParams
    {
        public IEnumerable<string> Options { get; set; }
        public string ModalTitle { get; set; }
    }


}

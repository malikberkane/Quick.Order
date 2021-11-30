
using System.Runtime.CompilerServices;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MalikBerkane.MvvmToolkit.Popup
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BasePopup
    {
        public BasePopup()
        {
            InitializeComponent();
            //ControlTemplate = this.Resources["ModalPopupControlTemplate"] as ControlTemplate;


            this.Dismissed += BasePopup_Dismissed;


            var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;

            //Animation = new ScaleAnimation()
            //{
            //    DurationIn = 150,
            //    DurationOut = 150,
            //    PositionIn = Rg.Plugins.Popup.Enums.MoveAnimationOptions.Bottom,
            //    PositionOut = Rg.Plugins.Popup.Enums.MoveAnimationOptions.Bottom
            //};
        }



        private void BasePopup_Dismissed(object sender, Xamarin.CommunityToolkit.UI.Views.PopupDismissedEventArgs e)
        {
            var modalBindingContext = BindingContext as ICancelableModal;

            if (modalBindingContext != null)
            {
                modalBindingContext.CancelModalTask();
            }

        }



        //protected override bool OnBackButtonPressed()
        //{

        //    var modalBindingContext = BindingContext as ICancelableModal;

        //    if (modalBindingContext != null)
        //    {
        //        modalBindingContext.CancelModalTask();
        //    }

        //    return true;
        //}

    }

}


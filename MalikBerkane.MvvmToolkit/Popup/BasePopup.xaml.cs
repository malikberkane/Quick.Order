using Rg.Plugins.Popup.Animations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            switch (Device.Idiom)
            {
                case TargetIdiom.Desktop:
                    ControlTemplate = Resources["DesktopPopupControlTemplate"] as ControlTemplate;
                    break;
                default:
                    ControlTemplate = Resources["ModalPopupControlTemplate"] as ControlTemplate;
                    break;
            }

            Animation = new ScaleAnimation()
            {
                DurationIn = 150,
                DurationOut = 150,
                PositionIn = Rg.Plugins.Popup.Enums.MoveAnimationOptions.Bottom,
                PositionOut = Rg.Plugins.Popup.Enums.MoveAnimationOptions.Bottom
            };
        }


        protected override bool OnBackButtonPressed()
        {

            var modalBindingContext = BindingContext as ICancelableModal;

            if (modalBindingContext != null)
            {
                modalBindingContext.CancelModalTask();
            }

            return true;
        }

    }

}


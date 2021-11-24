using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Quick.Order.Native.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WaitingForOrderPage : ContentPage
    {
        public WaitingForOrderPage()
        {
            InitializeComponent();
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                mediaElement.Play();

                mediaElement.ScaleTo(0.7f);

                return false;
            });
        }

      
    }

}

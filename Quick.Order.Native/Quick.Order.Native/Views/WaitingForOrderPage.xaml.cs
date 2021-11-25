using System;

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

            mediaElement.PropertyChanged += MediaElement_PropertyChanged;
        }

        private void MediaElement_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                mediaElement.Play();

                mediaElement.ScaleTo(0.7f);

                return false;
            });
        }


        protected override void OnDisappearing()
        {
            mediaElement.PropertyChanged -= MediaElement_PropertyChanged;

            base.OnDisappearing();
        }
    }

}

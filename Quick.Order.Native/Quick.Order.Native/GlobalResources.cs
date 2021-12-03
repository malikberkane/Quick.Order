using System.ComponentModel;
using Xamarin.Forms;

namespace Quick.Order.Native
{
    public class GlobalResources : INotifyPropertyChanged
    {
        public static GlobalResources Current = new GlobalResources();

        private double _thirdOfScreenWidth;
        public double ThirdOfScreenWidth
        {
            get { return _thirdOfScreenWidth; }
            set
            {
                _thirdOfScreenWidth = value;
                OnPropertyChanged(nameof(ThirdOfScreenWidth));
            }
        }


        private Thickness _listMargin;
        public Thickness ListMargin
        {
            get { return _listMargin; }
            set
            {
                _listMargin = value;
                OnPropertyChanged(nameof(ListMargin));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}

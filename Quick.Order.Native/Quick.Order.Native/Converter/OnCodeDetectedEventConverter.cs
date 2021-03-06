using GoogleVisionBarCodeScanner;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace Quick.Order.Native.Converter
{
    public class OnCodeDetectedEventConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var eventArgs = value as OnDetectedEventArg;
            return eventArgs.BarcodeResults[0].DisplayValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}

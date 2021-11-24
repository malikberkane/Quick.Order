using Quick.Order.AppCore.Models;
using Quick.Order.AppCore.Resources;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace Quick.Order.Native.Converter
{
    public class OrderStatusDescriptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            
            if(value is OrderStatus status)
            {
                switch (status)
                {
                    case OrderStatus.Pending:
                        return AppResources.OrderPending;
                    case OrderStatus.InProgress:
                        return AppResources.OrderInProgress;
                    case OrderStatus.Done:
                        return AppResources.OrderReady;
                    default:
                        return null;
                }
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}

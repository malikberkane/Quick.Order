using Newtonsoft.Json;
using Quick.Order.AppCore.Contracts;
using Quick.Order.Shared.Infrastructure.Exceptions;
using System;
using Xamarin.Essentials;
namespace Quick.Order.Shared.Infrastructure
{
    public class LocalHistoryService : ILocalHistoryService
    {
        private const string OrderIdKey = "CurrentOrderId";
        private const char SeparatorKey = '&';
        private const string LocalOrderKey = "LocalOrderKey";

        public void AddLocalPendingOrder(AppCore.Models.Order order)
        {
            Preferences.Set(OrderIdKey, $"{order.Id}{SeparatorKey}{order.OrderDate}");
        }

        public (string id, DateTime orderDate) GetLocalPendingOrder()
        {
            var orderStr = Preferences.Get(OrderIdKey, string.Empty);

            if (string.IsNullOrEmpty(orderStr))
            {
                return (string.Empty, default);
            }

            string[] properties = orderStr.Split(SeparatorKey);
            DateTime orderDate = default;
            if (properties.Length == 2 &&  DateTime.TryParse(properties[1], out  orderDate))
            {
                return (properties[0], orderDate);
            }
            else
            {
                throw new UnableToParseLocalOrderException();

            }

        }

        public void RemoveLocalPendingOrder()
        {
            Preferences.Remove(OrderIdKey, string.Empty);
        }


        public AppCore.Models.Order GetLocalOrder()
        {
            try
            {
                return JsonConvert.DeserializeObject<AppCore.Models.Order>(Preferences.Get(LocalOrderKey, string.Empty));

            }
            catch (System.Exception)
            {

                return null;
            }
        }

        public void SaveLocalOrder(AppCore.Models.Order order)
        {
            Preferences.Set(LocalOrderKey, JsonConvert.SerializeObject(order));

        }

        public void DeleteLocalOrder()
        {
            Preferences.Set(LocalOrderKey, string.Empty);

        }

    }
}

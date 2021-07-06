using Quick.Order.AppCore.Contracts;
using System.Threading.Tasks;
using Xamarin.Essentials;
namespace Quick.Order.Shared.Infrastructure
{
    public class LocalSettingsService : ILocalSettingsService
    {
        private const string OrderIdKey = "CurrentOrderId";

        public void AddLocalPendingOrder(AppCore.Models.Order order)
        {
            Preferences.Set(OrderIdKey, $"{order.Id}");
        }

        public string GetLocalPendingOrderId()
        {
           return Preferences.Get(OrderIdKey, string.Empty);
        }
    }
}

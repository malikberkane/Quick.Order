using System.Threading.Tasks;

namespace Quick.Order.AppCore.Contracts
{
    public interface ILocalSettingsService
    {
        string GetLocalPendingOrderId();
        void AddLocalPendingOrder(AppCore.Models.Order order);
    }
}

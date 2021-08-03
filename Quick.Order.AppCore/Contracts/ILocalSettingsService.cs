using System;
using System.Threading.Tasks;

namespace Quick.Order.AppCore.Contracts
{
    public interface ILocalSettingsService
    {
        (string id, DateTime orderDate) GetLocalPendingOrder();
        void AddLocalPendingOrder(AppCore.Models.Order order);

        void RemoveLocalPendingOrder();
    }
}

using Quick.Order.Native.ViewModels;

namespace Quick.Order.Native.Services
{
    public static class MappingService
    {
        public static AppCore.Models.Order VmToModel(this OrderVm orderVm)
        {
            return new AppCore.Models.Order
            {
                Id = orderVm.Id,
                RestaurantId = orderVm.RestaurantId,
                ClientName = orderVm.ClientName,
                OrderDate = orderVm.OrderDate,
                OrderStatus = orderVm.OrderStatus,
                OrderedItems = orderVm.OrderedItems,
                Note = orderVm.Note,
                TableNumber=orderVm.TableNumber
                
            };
        }

        public static OrderVm ModelToVm(this AppCore.Models.Order order)
        {
            return new OrderVm
            {
                Id = order.Id,
                RestaurantId = order.RestaurantId,
                ClientName = order.ClientName,
                OrderDate = order.OrderDate,
                OrderStatus = order.OrderStatus,
                OrderedItems = order.OrderedItems,
                Note = order.Note,
                TableNumber=order.TableNumber,
                OrderTotalPrice=order.OrderTotalPrice
            };
        }
    }
}
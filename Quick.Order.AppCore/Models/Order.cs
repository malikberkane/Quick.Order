using System;
using System.Collections.Generic;

namespace Quick.Order.AppCore.Models
{
    public class Order
    {
        public Guid Id { get; set; }= Guid.NewGuid();

        public Guid RestaurantId { get; set; }


        public string ClientName { get; set; }

        public DateTime OrderDate { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public List<BasketItem> OrderedItems { get; set; }
        public string Note { get; set; }

        public static Order CreateNew(Restaurant restaurant, IEnumerable<BasketItem> orderedItems, string generalNote = null)
        {
            return new Order
            {
                OrderDate = DateTime.Now,
                OrderedItems = new List<BasketItem>(orderedItems),
                RestaurantId = restaurant.Id,
                Note = generalNote
            };
        }
    }

}

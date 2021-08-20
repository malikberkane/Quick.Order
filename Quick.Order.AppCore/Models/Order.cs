using System;
using System.Collections.Generic;
using System.Linq;

namespace Quick.Order.AppCore.Models
{
    public class Order
    {
        public Guid Id { get; set; }= Guid.NewGuid();

        public Guid RestaurantId { get; set; }


        public string ClientName { get; set; }
        public string TableNumber { get; set; }

        public DateTime OrderDate { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public List<BasketItem> OrderedItems { get; set; }


        public double OrderTotalPrice => OrderedItems != null && OrderedItems.Any() ? OrderedItems.Sum(i => i.ItemPriceValue) : 0;
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


        public bool IsValid()
        {
            return !string.IsNullOrEmpty(ClientName) && OrderedItems != null && OrderedItems.Any();
        }


        public override bool Equals(object obj)
        {
            if(obj is Order other)
            {
                return other.Id == this.Id;
            }
            return false;
        }

      
    }


    public class OrderEqualityComparer : IEqualityComparer<Order>
    {
        
        public bool Equals(Order x, Order y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(Order obj)
        {
            return base.GetHashCode();
        }
    }

}

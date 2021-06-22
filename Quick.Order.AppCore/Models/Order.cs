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

        public List<OrderUnit> OrderedItems { get; set; }
        public string Note { get; set; }

    }

}

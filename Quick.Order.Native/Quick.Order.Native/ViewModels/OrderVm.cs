using Microsoft.Toolkit.Mvvm.ComponentModel;
using Quick.Order.AppCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quick.Order.Native.ViewModels
{
    public class OrderVm: ObservableObject
    {
        public Guid Id { get; set; } 

        public Guid RestaurantId { get; set; }


        public string ClientName { get; set; }

        public DateTime OrderDate { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public List<BasketItem> OrderedItems { get; set; }
        public string Note { get; set; }

    }
}

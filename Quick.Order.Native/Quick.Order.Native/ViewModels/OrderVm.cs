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
        public string TableNumber { get; set; }

        public List<BasketItem> OrderedItems { get; set; }
        public string Note { get; set; }

        public double OrderTotalPrice { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is OrderVm other)
            {
                return other.Id == this.Id;
            }
            return false;
        }


    }
}

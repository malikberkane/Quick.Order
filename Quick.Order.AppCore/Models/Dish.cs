using System;
using System.Collections.Generic;

namespace Quick.Order.AppCore.Models
{
    public class Dish
    {

        public string Name { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }
        public override bool Equals(object obj)
        {
            if (obj is Dish other)
            {
                return other.Name == Name;
            }
            return base.Equals(obj);
        }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Description) && Price != 0;
        }
    }   

}

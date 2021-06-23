using System;
using System.Collections.Generic;

namespace Quick.Order.AppCore.Models
{
    public class Dish
    {

        public string Name { get; set; }
        public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();

        public override bool Equals(object obj)
        {
            if (obj is Dish other)
            {
                return other.Name == Name;
            }
            return base.Equals(obj);
        }
    }   

}

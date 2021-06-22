using System;
using System.Collections.Generic;

namespace Quick.Order.AppCore.Models
{
    public class Dish
    {

        public Guid Id { get; set; } =Guid.NewGuid();
        public string Name { get; set; }
        public List<Ingredient> Ingredients {get;set;}
    }   

    public class DishSection
    {
        public string Name { get; set; }
        public List<Dish> Dishes { get; set; }
    }

}

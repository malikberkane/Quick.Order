using Quick.Order.AppCore.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace Quick.Order.AppCore.Models
{
    public class DishSection
    {
        public string Name { get; set; }
        public List<Dish> Dishes { get; set; }= new List<Dish>();


        public void AddDish(Dish dish)
        {
            if (Dishes == null)
            {
                Dishes = new List<Dish>();
            }

            if (Dishes.Any(s => s.Equals(dish)))
            {
                throw new ExistingDishException();
            }

            Dishes.Add(dish);
        }
        public override bool Equals(object obj)
        {
            if(obj is DishSection other)
            {
                return other.Name == Name;
            }
            return base.Equals(obj);
        }
    }

}

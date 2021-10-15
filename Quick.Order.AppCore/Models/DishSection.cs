using Quick.Order.AppCore.Exceptions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Quick.Order.AppCore.Models
{
    public class DishSection
    {
        public DishSection(string name, IEnumerable<Dish> dishes=null)
        {
            Name = name;
            Dishes = dishes!=null ? new List<Dish>(dishes) : new List<Dish>();
        }

        public string Name { get; set; }
        private List<Dish> Dishes = new List<Dish>();

        public ReadOnlyCollection<Dish> GetDishes()
        {
            return new ReadOnlyCollection<Dish>(Dishes);
        }
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


        public void Remove(Dish dish)
        {
            
            Dishes.Remove(dish);
        }

        public void UpdateDish(Dish oldDish, Dish newDish)
        {


            var dishToEditIndex = Dishes.IndexOf(oldDish);
            if (dishToEditIndex != -1)
            {
                for (int i = 0; i < Dishes.Count-1; i++)
                {
                    if(i!=dishToEditIndex && Dishes[i].Equals(newDish))
                    {
                        throw new System.Exception("Dish with same name");

                    }
                }

                Dishes[dishToEditIndex] = newDish;
            }
            else
            {
                throw new System.Exception("Dish to edit not found");
            }

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

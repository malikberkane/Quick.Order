using Quick.Order.AppCore.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace Quick.Order.AppCore.Models
{
    public class Menu
    {

        public List<DishSection> Sections { get; set; }

        public void AddSection(DishSection section)
        {
            if (Sections == null)
            {
                Sections = new List<DishSection>();
            }

            if (Sections.Any(s => s.Equals(section)))
            {
                throw new ExistingDishSectionException();
            }

            Sections.Add(section);
        }


         

        public static Menu CreateDefault()
        {
            return new Menu { Sections = new List<DishSection> { new DishSection { Name = "Entrées" }, new DishSection { Name = "Plats" }, new DishSection { Name = "Desserts" } } };
        }
        public void AddDishToSection(Dish dish, DishSection section)
        {

            var sectionToUpdate = Sections.SingleOrDefault(s => s.Equals(section));

            if (section == null)
                throw new SectionNotFoundException();

            sectionToUpdate.AddDish(dish);



        }
    }
}

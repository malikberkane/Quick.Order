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

        public static Menu CreateEmpty()
        {
            return new Menu { Sections = new List<DishSection>() };
        }
        public void AddDishToSection(Dish dish, DishSection section)
        {

            var sectionToUpdate = Sections.SingleOrDefault(s => s.Equals(section));

            if (section == null)
                throw new SectionNotFoundException();

            sectionToUpdate.AddDish(dish);


        }

        public void UpdateDishSection(string oldName, string newName)
        {
            var dishSectionToEdit = Sections.FindIndex(s=>s.Name==oldName);
            if (dishSectionToEdit != -1)
            {
                

                Sections[dishSectionToEdit].Name = newName;
            }
            else
            {
                throw new System.Exception("Dish section to edit not found");
            }
        }


        public void DeleteDishSection(DishSection section)
        {
            Sections.Remove(section);
        }
        public void UpdateDishSection(DishSection oldSection, DishSection newSection)
        {
            var dishSectionToEdit = Sections.IndexOf(oldSection);
            if (dishSectionToEdit != -1)
            {


                Sections[dishSectionToEdit] = newSection;
            }
            else
            {
                throw new System.Exception("Dish section to edit not found");
            }
        }

        public DishSection GetDishSection(Dish dish)
        {
            return Sections.FirstOrDefault(s => s.Dishes.Contains(dish));
        }


        public DishSection GetDishSectionByName(string name)
        {
            return Sections.SingleOrDefault(n => n.Name == name);
        }

    }
}

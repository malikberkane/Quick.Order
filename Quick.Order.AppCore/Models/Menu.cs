using Quick.Order.AppCore.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Quick.Order.AppCore.Models
{
    public class Menu
    {

        private List<DishSection> _sections;

        public IReadOnlyCollection<DishSection> Sections => new ReadOnlyCollection<DishSection>(_sections);

        public void AddSection(DishSection section)
        {
            if (_sections == null)
            {
                _sections = new List<DishSection>();
            }

            if (_sections.Any(s => s.Equals(section)))
            {
                throw new ExistingDishSectionException();
            }

            _sections.Add(section);
        }

        public void RemoveAllSections(Predicate<DishSection> predicate)
        {
            _sections.RemoveAll(predicate);
        }


        public static Menu CreateDefault()
        {
            return new Menu { _sections = new List<DishSection> { new DishSection("Entrées"), new DishSection("Plats"), new DishSection("Desserts") } };
        }

        public static Menu CreateEmpty()
        {
            return new Menu { _sections = new List<DishSection>() };
        }
        public void AddDishToSection(Dish dish, DishSection section)
        {

            var sectionToUpdate = _sections.SingleOrDefault(s => s.Equals(section));

            if (section == null)
                throw new SectionNotFoundException();

            sectionToUpdate.AddDish(dish);


        }

        public void UpdateDishSection(string oldName, string newName)
        {
            var dishSectionToEdit = _sections.FindIndex(s=>s.Name==oldName);
            if (dishSectionToEdit != -1)
            {
                

                _sections[dishSectionToEdit].Name = newName;
            }
            else
            {
                throw new System.Exception("Dish section to edit not found");
            }
        }


        public void DeleteDishSection(DishSection section)
        {
            _sections.Remove(section);
        }
        public void UpdateDishSection(DishSection oldSection, DishSection newSection)
        {
            var dishSectionToEdit = _sections.IndexOf(oldSection);
            if (dishSectionToEdit != -1)
            {


                _sections[dishSectionToEdit] = newSection;
            }
            else
            {
                throw new System.Exception("Dish section to edit not found");
            }
        }

        public DishSection GetDishSection(Dish dish)
        {
            return _sections.FirstOrDefault(s => s.GetDishes().Contains(dish));
        }


        public DishSection GetDishSectionByName(string name)
        {
            return _sections.SingleOrDefault(n => n.Name == name);
        }

    }
}

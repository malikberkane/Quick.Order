using Quick.Order.AppCore.Exceptions;
using Quick.Order.AppCore.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Quick.Order.Native.ViewModels
{
    public class DishSectionGroupedModel: ObservableCollection<Dish>
    {
        public string SectionName { get; set; }

        public void AddDish(Dish dish)
        {
           

            if (this.Any(s => s.Equals(dish)))
            {
                throw new ExistingDishException();
            }

            this.Add(dish);
        }

        public void UpdateDish(Dish oldDish, Dish newDish)
        {
            var dishToEditIndex = this.IndexOf(oldDish);
            if (dishToEditIndex != -1)
            {
                for (int i = 0; i < this.Count - 1; i++)
                {
                    if (i != dishToEditIndex && this[i].Equals(newDish))
                    {
                        throw new System.Exception("Dish with same name");

                    }
                }

                this[dishToEditIndex] = newDish;
            }
            else
            {
                throw new System.Exception("Dish to edit not found");
            }

        }    


        public void EditSection(string newName)
        {
            SectionName = newName;
        }
    }


    public class DishSectionGroupedModelCollection: ObservableCollection<DishSectionGroupedModel>
    {
        public void AddDishToSection(string sectionName, Dish dish)
        {
            var section = this.SingleOrDefault(n => n.SectionName == sectionName);
            if (section != null)
            {
                section.AddDish(dish);
            }
        }

        public void UpdateDish(string sectionName,Dish oldDish, Dish newDish)
        {
            var section = this.SingleOrDefault(n => n.SectionName == sectionName);
            if (section != null)
            {
                section.UpdateDish(oldDish, newDish);
     
            }

        }


        public void UpdateDish(Dish oldDish, Dish newDish)
        {
            var section = this.FirstOrDefault(n => n.Contains(oldDish));
            if (section != null)
            {
                section.UpdateDish(oldDish, newDish);

            }

        }


        public void RemoveDish(string sectionName, Dish dishToRemove)
        {
            var section = this.SingleOrDefault(n => n.SectionName == sectionName);
            if (section != null)
            {
                section.Remove(dishToRemove);

            }

        }


        public void RemoveDish(Dish dishToRemove)
        {
            var section = this.FirstOrDefault(n => n.Contains(dishToRemove));
            if (section != null)
            {
                section.Remove(dishToRemove);

            }

        }


        public void EditSection(string oldName, string newName)
        {

            var section = this.SingleOrDefault(n => n.SectionName == oldName);
            if (section != null)
            {
                section.EditSection(newName);

            }
        }

        public void RemoveSection(string sectionName)
        {
            var section = this.SingleOrDefault(n => n.SectionName == sectionName);
            if (section != null)
            {
                this.Remove(section);

            }
        }

        public void AddDishSection(DishSection section)
        {
            this.Add(new DishSectionGroupedModel { SectionName = section.Name });
        }

    }


    public class DishSectionGroupedModelList : List<DishSectionGroupedModel>
    {
        public void AddDishToSection(string sectionName, Dish dish)
        {
            var section = this.SingleOrDefault(n => n.SectionName == sectionName);
            if (section != null)
            {
                section.AddDish(dish);
            }
        }

        public void UpdateDish(string sectionName, Dish oldDish, Dish newDish)
        {
            var section = this.SingleOrDefault(n => n.SectionName == sectionName);
            if (section != null)
            {
                section.UpdateDish(oldDish, newDish);

            }

        }


        public void UpdateDish(Dish oldDish, Dish newDish)
        {
            var section = this.FirstOrDefault(n => n.Contains(oldDish));
            if (section != null)
            {
                section.UpdateDish(oldDish, newDish);

            }

        }


        public void RemoveDish(string sectionName, Dish dishToRemove)
        {
            var section = this.SingleOrDefault(n => n.SectionName == sectionName);
            if (section != null)
            {
                section.Remove(dishToRemove);

            }

        }


        public void RemoveDish(Dish dishToRemove)
        {
            var section = this.FirstOrDefault(n => n.Contains(dishToRemove));
            if (section != null)
            {
                section.Remove(dishToRemove);

            }

        }


        public void EditSection(string oldName, string newName)
        {

            var section = this.SingleOrDefault(n => n.SectionName == oldName);
            if (section != null)
            {
                section.EditSection(newName);

            }
        }

        public void RemoveSection(string sectionName)
        {
            var section = this.SingleOrDefault(n => n.SectionName == sectionName);
            if (section != null)
            {
                this.Remove(section);

            }
        }

        public void AddDishSection(DishSection section)
        {
            this.Add(new DishSectionGroupedModel { SectionName = section.Name });
        }

    }

}


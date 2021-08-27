using System;

namespace Quick.Order.AppCore.Models
{
    public class Restaurant
    {

        public Restaurant(string name, string adresse, Menu menu)
        {
            Name = name;
            Adresse = adresse;
            Menu = menu;
        }

        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; private set; }
        public string Adresse { get; private set; }

        public RestaurantAdmin Administrator { get; set; }
        public bool IsOpen { get; set; }

        public Menu Menu { get; set; }
        public void SetAdministator(RestaurantAdmin admin)
        {
            Administrator = admin;
        }


        public void AddDishSectionToMenu(DishSection section)
        {
            if (Menu == null)
            {
                Menu = Menu.CreateEmpty();
            }
            Menu.AddSection(section);
        }

        


        public void AddDishToMenu(Dish dish, DishSection section=null)
        {
            
            Menu.AddDishToSection(dish, section);

        }

        public bool IsDescriptionValid()
        {
            return !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Adresse);
        }


        public void EditIdentity(string name, string adress)
        {
            Name = name;
            Adresse = adress;

        }


        
       
    }
}

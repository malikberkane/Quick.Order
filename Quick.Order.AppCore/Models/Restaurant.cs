using System;

namespace Quick.Order.AppCore.Models
{
    public class Restaurant
    {
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

        public Restaurant(string name, string adresse)
        {
            Name = name;
            Adresse = adresse;
            Menu = new Menu { Dishes = new System.Collections.Generic.List<DishSection>() };
        }
    }
}

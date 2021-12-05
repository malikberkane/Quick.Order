using System;

namespace Quick.Order.AppCore.Models
{
    public class Restaurant
    {

        public const string DefaultRestaurantPictureSource = "https://firebasestorage.googleapis.com/v0/b/quickorder-f339b.appspot.com/o/default_restaurant_picture.png?alt=media&token=1f7e29da-03c9-4456-a05f-072e7a864e1c";
        public Restaurant(string name, string adresse, Menu menu, RestaurantAdmin admin, string photoSource= DefaultRestaurantPictureSource, Guid guid=default, Currency currency=null)
        {
            Name = name;
            Adresse = adresse;
            Menu = menu;
            Administrator = admin;
            Id = guid == default ? Guid.NewGuid() : guid;
            RestaurantPhotoSource = photoSource;
            Currency = currency ?? new Currency { Code = "USD", Symbol = "$", Name = "United States dollar" };
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Adresse { get; private set; }

        public Currency Currency { get; set; }
        public RestaurantAdmin Administrator { get; private set; }
        public bool IsOpen { get; set; }

        public string RestaurantPhotoSource { get; set; } 

        public Menu Menu { get; private set; }

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


        public void EditIdentity(string name, string adress, Currency currency)
        {
            Name = name;
            Adresse = adress;
            Currency = currency;
        }


        
       
    }


}

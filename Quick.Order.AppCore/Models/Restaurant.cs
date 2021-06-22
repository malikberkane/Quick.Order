using System;

namespace Quick.Order.AppCore.Models
{
    public class Restaurant
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Adresse { get; set; }

        public RestaurantAdmin Administrator { get; set; }
        public bool IsOpen { get; set; }

        public Menu Menu { get; set; }
        public void SetAdministator(RestaurantAdmin admin)
        {
            Administrator = admin;
        }
    }
}

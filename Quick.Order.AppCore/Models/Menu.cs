using System.Collections.Generic;

namespace Quick.Order.AppCore.Models
{
    public class Menu
    {
        public Restaurant Restaurant { get; set; }

        public List<DishSection> Dishes { get; set; } = new List<DishSection>();

    }
}

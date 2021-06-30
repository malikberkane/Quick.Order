namespace Quick.Order.AppCore.Models
{
    public class BasketItem
    {
        public Dish Dish { get; set; }

        public int Quantity { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is BasketItem other)
            {
                return other.Dish!=null && other.Dish.Equals(this.Dish);
            }
            return base.Equals(obj);
        }
    }

}

namespace Quick.Order.AppCore.Models
{
    public class RestaurantAdmin
    {
        public string Name { get; set; }

        public string Email { get; set; }
        public string UserId => Email;

        public override bool Equals(object obj)
        {
            if(obj is RestaurantAdmin other)
            {
                return other.UserId == this.UserId;
            }
            return base.Equals(obj);
        }
    }
}

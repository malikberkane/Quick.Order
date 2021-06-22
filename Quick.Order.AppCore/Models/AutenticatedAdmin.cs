namespace Quick.Order.AppCore.Models
{
    public class AutenticatedRestaurantAdmin
    {
        public RestaurantAdmin RestaurantAdmin { get; set; }
        public string AuthenticationToken { get; set; }

        public bool AuthenticationExpired { get; set; }

    }
}

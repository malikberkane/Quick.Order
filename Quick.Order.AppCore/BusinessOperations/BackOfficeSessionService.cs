using Quick.Order.AppCore.Models;

namespace Quick.Order.AppCore.BusinessOperations
{
    public class BackOfficeSessionService
    {
        public  Restaurant CurrentRestaurantSession { get; set; }

        public string CurrentUser { get; set; }
    }
}

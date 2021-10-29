using Quick.Order.AppCore.Contracts.Repositories;

namespace Quick.Order.AppCore.ServicesAggregate
{
    public class RepositoriesAggregate
    {
        public RepositoriesAggregate(IRestaurantRepository restaurantRepository, IOrdersRepository ordersRepository)
        {
            Restaurants = restaurantRepository;
            Orders = ordersRepository;
        }
        public IRestaurantRepository Restaurants { get; }

        public IOrdersRepository Orders { get; }

    }
}

using Quick.Order.AppCore.Contracts.Repositories;

namespace Quick.Order.AppCore.ServicesAggregate
{
    public class RepositoriesAggregate
    {
        public RepositoriesAggregate(IRestaurantRepository restaurantRepository, IOrdersRepository ordersRepository, ICurrencyRepository currencyRepository)
        {
            Restaurants = restaurantRepository;
            Orders = ordersRepository;
            Currencies = currencyRepository;
        }
        public IRestaurantRepository Restaurants { get; }

        public IOrdersRepository Orders { get; }

        public ICurrencyRepository Currencies { get; }

    }
}

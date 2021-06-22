using Quick.Order.AppCore.Contracts.Repositories;
using Quick.Order.AppCore.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Quick.Order.AppCore.BusinessOperations
{
    public class FrontOfficeRestaurantService
    {
        private readonly IRestaurantRepository restaurantRepository;

        public FrontOfficeRestaurantService(IRestaurantRepository restaurantRepository)
        {
            this.restaurantRepository = restaurantRepository;
        }
        public Task<IEnumerable<Restaurant>> GetAllRestaurants()
        {
            return restaurantRepository.Get();
        }

        public Task<Restaurant> GetRestaurantById(Guid id)
        {
            return restaurantRepository.GetById(id);
        }

        public Task<Restaurant> GetRestaurantCloseBy(string adress, int maxDistance,int count)
        {
            return null;
        }
    }


}

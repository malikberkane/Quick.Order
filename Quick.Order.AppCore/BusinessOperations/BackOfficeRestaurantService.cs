using Quick.Order.AppCore.Authentication.Contracts;
using Quick.Order.AppCore.Contracts;
using Quick.Order.AppCore.Contracts.Repositories;
using Quick.Order.AppCore.Exceptions;
using Quick.Order.AppCore.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Quick.Order.AppCore.BusinessOperations
{
    public class BackOfficeRestaurantService
    {
        private readonly IRestaurantRepository restaurantRepository;
        private readonly IOrdersRepository ordersRepository;
        private readonly BackOfficeSessionService backOfficeSessionService;
        private readonly IConnectivityService connectivityService;

        public BackOfficeRestaurantService(IRestaurantRepository restaurantRepository,  IOrdersRepository ordersRepository, BackOfficeSessionService backOfficeSessionService, IConnectivityService connectivityService)
        {
            this.restaurantRepository = restaurantRepository;
            this.ordersRepository = ordersRepository;
            this.backOfficeSessionService = backOfficeSessionService;
            this.connectivityService = connectivityService;
        }
        public async Task<Restaurant> AddRestaurant(Restaurant restaurant)
        {
            if (restaurant.Administrator == null)
            {
                throw new SettingAdminForNewRestaurantException();
            }

            if (!connectivityService.HasNetwork())
            {
                throw new NoNetworkException();

            }

            var createdRestaurant= await restaurantRepository.Add(restaurant);

            backOfficeSessionService.CurrentRestaurantSession = createdRestaurant;
            return createdRestaurant;
        }

        public async Task<bool> DeleteRestaurant(Restaurant restaurant)
        {
            if (!connectivityService.HasNetwork())
            {
                throw new NoNetworkException();

            }
            var result= await restaurantRepository.Delete(restaurant);
            backOfficeSessionService.CurrentRestaurantSession = null;
            return result;
        }

        public Task<bool> UpdateRestaurant(Restaurant restaurant)
        {
            if (!connectivityService.HasNetwork())
            {
                throw new NoNetworkException();

            }
            return restaurantRepository.Update(restaurant);
        }

       
        public Task<List<Models.Order>> GetOrdersForRestaurant(Guid restaurantId)
        {
            if (!connectivityService.HasNetwork())
            {
                throw new NoNetworkException();

            }
            return ordersRepository.GetOrdersForRestaurant(restaurantId);
        }


        public Task SetOrderStatus(Models.Order order, OrderStatus status)
        {
            if (!connectivityService.HasNetwork())
            {
                throw new NoNetworkException();

            }
            order.OrderStatus = status;
            return ordersRepository.Update(order);


        }

        public Task<Models.Order> GetOrder(Guid id)
        {
            if (!connectivityService.HasNetwork())
            {
                throw new NoNetworkException();

            }
            return ordersRepository.GetById(id);

        }


        public Task<bool> DeleteOrder(Models.Order order)
        {
            if (!connectivityService.HasNetwork())
            {
                throw new NoNetworkException();

            }
            return ordersRepository.Delete(order);

        }

       
    }
}

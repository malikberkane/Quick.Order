using Quick.Order.AppCore.Authentication.Contracts;
using Quick.Order.AppCore.Contracts.Repositories;
using Quick.Order.AppCore.Exceptions;
using Quick.Order.AppCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quick.Order.AppCore.BusinessOperations
{
    public class BackOfficeRestaurantService
    {
        private readonly IRestaurantRepository restaurantRepository;
        private readonly IAuthenticationService authenticationService;
        private readonly IOrdersRepository ordersRepository;
        private readonly BackOfficeSessionService backOfficeSessionService;

        public BackOfficeRestaurantService(IRestaurantRepository restaurantRepository, IAuthenticationService authenticationService, IOrdersRepository ordersRepository, BackOfficeSessionService backOfficeSessionService)
        {
            this.restaurantRepository = restaurantRepository;
            this.authenticationService = authenticationService;
            this.ordersRepository = ordersRepository;
            this.backOfficeSessionService = backOfficeSessionService;
        }
        public async Task<Restaurant> AddRestaurant(Restaurant restaurant)
        {
            if (restaurant.Administrator == null && authenticationService.LoggedUser!=null)
            {
                restaurant.SetAdministator(authenticationService.LoggedUser.RestaurantAdmin);
            }
            else
            {
                throw new SettingAdminForNewRestaurantException();
            }
            var createdRestaurant= await restaurantRepository.Add(restaurant);

            backOfficeSessionService.CurrentRestaurantSession = createdRestaurant;
            return createdRestaurant;
        }

        public async Task<bool> DeleteRestaurant(Restaurant restaurant)
        {
            var result= await restaurantRepository.Delete(restaurant);
            backOfficeSessionService.CurrentRestaurantSession = null;
            return result;
        }

        public Task<bool> UpdateRestaurant(Restaurant restaurant)
        {
            return restaurantRepository.Update(restaurant);
        }

       







        public Task<List<AppCore.Models.Order>> GetOrdersForRestaurant(Guid restaurantId)
        {
            
            return ordersRepository.GetOrdersForRestaurant(restaurantId);
        }


        public Task SetOrderStatus(Models.Order order, OrderStatus status)
        {
            order.OrderStatus = status;
            return ordersRepository.Update(order);


        }

        public Task<AppCore.Models.Order> GetOrder(Guid id)
        {
            return ordersRepository.GetById(id);

        }


        public Task<bool> DeleteOrder(AppCore.Models.Order order)
        {
            return ordersRepository.Delete(order);

        }
    }
}

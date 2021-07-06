using Quick.Order.AppCore.Contracts;
using Quick.Order.AppCore.Contracts.Repositories;
using Quick.Order.AppCore.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
namespace Quick.Order.AppCore.BusinessOperations
{
    public class FrontOfficeRestaurantService
    {
        private readonly IRestaurantRepository restaurantRepository;
        private readonly IOrdersRepository ordersRepository;
        private readonly IEmailService emailService;

        public FrontOfficeRestaurantService(IRestaurantRepository restaurantRepository, IOrdersRepository ordersRepository, IEmailService emailService)
        {
            this.restaurantRepository = restaurantRepository;
            this.ordersRepository = ordersRepository;
            this.emailService = emailService;
        }
        public Task<IEnumerable<Restaurant>> GetAllRestaurants()
        {
            return restaurantRepository.Get();
        }

        public Task<Restaurant> GetRestaurantById(Guid id)
        {
            return restaurantRepository.GetById(id);
        }

        public async Task PlaceOrder(Models.Order order)
        {
            var result = await ordersRepository.Add(order);

            if (result != null)
            {

               await emailService.SendEmailForOrder(order);
            }
        }

        public Task<Restaurant> GetRestaurantCloseBy(string adress, int maxDistance,int count)
        {
            return null;
        }


        public Task<AppCore.Models.Order> GetOrderStatuts(AppCore.Models.Order order)
        {
            return ordersRepository.GetById(order.Id);
        }

    }


}

using Quick.Order.AppCore.Contracts;
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
        private readonly IOrdersRepository ordersRepository;
        private readonly IEmailService emailService;
        private readonly ILocalHistoryService localSettingsService;

        public FrontOfficeRestaurantService(IRestaurantRepository restaurantRepository, IOrdersRepository ordersRepository, IEmailService emailService, ILocalHistoryService localSettingsService)
        {
            this.restaurantRepository = restaurantRepository;
            this.ordersRepository = ordersRepository;
            this.emailService = emailService;
            this.localSettingsService = localSettingsService;
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
               localSettingsService.AddLocalPendingOrder(order);
               localSettingsService.DeleteLocalOrder();
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

        public Task<AppCore.Models.Order> GetOrder(string id)
        {            
            var guid = Guid.Parse(id);
            return ordersRepository.GetById(guid);
        }

        public Task<AppCore.Models.Order> GetOrder(Guid id)
        {
            return ordersRepository.GetById(id);
        }

    }


}

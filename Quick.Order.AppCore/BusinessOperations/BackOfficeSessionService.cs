using Quick.Order.AppCore.Authentication.Contracts;
using Quick.Order.AppCore.Contracts;
using Quick.Order.AppCore.Contracts.Repositories;
using Quick.Order.AppCore.Exceptions;
using Quick.Order.AppCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quick.Order.AppCore.BusinessOperations
{
    public class BackOfficeSessionService
    {
        private readonly IRestaurantRepository restaurantRepository;
        private readonly ILoggerService loggerService;

        public BackOfficeSessionService(IRestaurantRepository restaurantRepository, ILoggerService loggerService)
        {
            this.restaurantRepository = restaurantRepository;
            this.loggerService = loggerService;
        }
        public  Restaurant CurrentRestaurantSession { get; set; }



        public async Task SetRestaurantForSession(RestaurantAdmin restaurantAdmin)
        {
            var restaurantsForAccount = await GetAllRestaurantsForAccount(restaurantAdmin);

            if (restaurantsForAccount != null && restaurantsForAccount.Any())
            {
                var restaurantForSession = restaurantsForAccount.First();

                if (restaurantForSession?.Menu?.Sections != null && restaurantForSession.Menu.Sections.Any(n => n == null))
                {
                    loggerService.Log(new Exception("Some menu sections are null"));
                    restaurantForSession.Menu.Sections.RemoveAll(n => n == null);

                }


                CurrentRestaurantSession = restaurantForSession;

            }
           
        }

        private Task<IEnumerable<Restaurant>> GetAllRestaurantsForAccount(RestaurantAdmin restaurantAdmin)
        {
            if (restaurantAdmin == null)
            {
                throw new UserNotLoggedException();
            }
            return restaurantRepository.Get(r => r.Administrator.Equals(restaurantAdmin));
        }


    }
}

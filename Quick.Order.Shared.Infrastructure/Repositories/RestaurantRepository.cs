using Firebase.Database;
using Quick.Order.AppCore.Contracts.Repositories;
using Quick.Order.AppCore.Models;
using System;
using Firebase.Database.Query;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Quick.Order.AppCore.Exceptions;
using Quick.Order.AppCore.Contracts;

namespace Quick.Order.Shared.Infrastructure.Repositories
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly ILoggerService loggerService;
        FirebaseClient firebase;

        public RestaurantRepository(ILoggerService loggerService)
        {
            firebase = new FirebaseClient("https://quickorder-f339b-default-rtdb.europe-west1.firebasedatabase.app/");
            this.loggerService = loggerService;
        }
        public async Task<Restaurant> Add(Restaurant item)
        {
            var result = await firebase
                              .Child("Restaurants")
                              .PostAsync(item);


            if (result.Object == null)
            {
                throw new Exception("Error savings");
            }

            return result.Object;
        }

        public Task<int> Count()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Delete(Restaurant item)
        {
            var DeleteUserDb = (await firebase
                      .Child("Restaurants")
                      .OnceAsync<Restaurant>()).SingleOrDefault(a => a.Object.Id == item.Id);
            await firebase.Child("Restaurants").Child(DeleteUserDb.Key).DeleteAsync();

            if (DeleteUserDb?.Object == null)
            {
                throw new Exception("Error deleting");
            }

            return true;
        }

        public async Task<IEnumerable<Restaurant>> Get()
        {

            var restaurants = (await firebase
                             .Child("Restaurants")
                             .OnceAsync<Restaurant>()).Select(item => item.Object);

            if (restaurants != null)
            {
                return new List<Restaurant>(restaurants);
            }
            else
            {
                throw new Exception("Error loading");
            }
        }

        public async Task<IEnumerable<Restaurant>> Get(Func<Restaurant, bool> func)
        {

            var restaurants = (await firebase
                            .Child("Restaurants")

                            .OnceAsync<Restaurant>()).Select(item => item.Object).Where(r => func(r));

            if (restaurants != null)
            {
                return new List<Restaurant>(restaurants);
            }
            else
            {
                throw new Exception("Error loading");
            }



        }

        public async Task<Restaurant> GetById(Guid id)
        {
            var restaurants = (await firebase
                                        .Child("Restaurants").OnceAsync<Restaurant>()).Where(a => a.Object.Id == id).FirstOrDefault();

            if (restaurants?.Object != null)
            {
                if (restaurants.Object.Menu?.Sections != null && restaurants.Object.Menu.Sections.Any(n=>n==null))
                {
                    loggerService.Log(new Exception("Some menu sections are null"));
                    restaurants.Object.Menu.Sections.RemoveAll(n => n == null);

                }

                return restaurants.Object;
            }
            else
            {
                throw new RestaurantNotFoundException();
            }
        }

        public async Task<bool> Update(Restaurant item)
        {
            var restaurantToUpdate = (await firebase
                                  .Child("Restaurants")
                                  .OnceAsync<Restaurant>()).Where(a => a.Object.Id == item.Id).FirstOrDefault();

            if (restaurantToUpdate?.Object == null)
            {
                throw new Exception("Error updating");
            }

            await firebase
              .Child("Restaurants")
              .Child(restaurantToUpdate.Key)
              .PutAsync(item);

            return true;
        }
    }
}

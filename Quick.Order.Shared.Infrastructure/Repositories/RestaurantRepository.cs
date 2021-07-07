using Firebase.Database;
using Quick.Order.AppCore.Contracts.Repositories;
using Quick.Order.AppCore.Models;
using System;
using Firebase.Database.Query;

using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Quick.Order.AppCore.Exceptions;

namespace Quick.Order.Shared.Infrastructure.Repositories
{
    public class RestaurantRepository : IRestaurantRepository
    {
        FirebaseClient firebase;

        public RestaurantRepository()
        {
            firebase = new FirebaseClient("https://quickorder-f339b-default-rtdb.europe-west1.firebasedatabase.app/");


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
            if (DeleteUserDb.Object == null)
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

        public async Task<IEnumerable<Restaurant>> Get(Func<Restaurant,bool> func)
        {

            try
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
            catch (Exception ex)
            {

                throw;
            }

        
        }

        public async Task<Restaurant> GetById(Guid id)
        {
            var restaurants = (await firebase
                                        .Child("Restaurants").OnceAsync<Restaurant>()).Where(a => a.Object.Id == id).FirstOrDefault();

            if (restaurants?.Object != null)
            {
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

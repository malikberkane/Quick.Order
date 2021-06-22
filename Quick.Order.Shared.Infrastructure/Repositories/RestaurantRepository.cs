using Firebase.Database;
using Quick.Order.AppCore.Contracts.Repositories;
using Quick.Order.AppCore.Models;
using System;
using Firebase.Database.Query;

using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

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

        public Task<bool> Delete(Restaurant item)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Restaurant>> Get()
        {

            var restaurants = (await firebase
                             .Child("Restaurants")
                             .OnceAsync<Restaurant>()).Select(item => item.Object).ToList();

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

            if (restaurants.Object != null)
            {
                return restaurants.Object;
            }
            else
            {
                throw new Exception("Error loading");
            }
        }

        public Task<bool> Update(Restaurant item)
        {
            throw new NotImplementedException();
        }
    }
}

using Firebase.Database;
using Quick.Order.AppCore.Contracts.Repositories;
using System;
using Firebase.Database.Query;

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Quick.Order.Shared.Infrastructure.Repositories
{
    public class OrdersRepository : IOrdersRepository
    {

        FirebaseClient firebase;

        public OrdersRepository()
        {
            firebase = new FirebaseClient("https://quickorder-f339b-default-rtdb.europe-west1.firebasedatabase.app/");

        }
        public async Task<AppCore.Models.Order> Add(AppCore.Models.Order item)
        {
            var result = await firebase.Child("Orders")
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

        public Task<bool> Delete(AppCore.Models.Order item)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AppCore.Models.Order>> Get()
        {
            throw new NotImplementedException();
        }

        public async Task<AppCore.Models.Order> GetById(Guid id)
        {
            var order = (await firebase.Child("Orders").OnceAsync<AppCore.Models.Order>()).Where(a => a.Object.Id == id).FirstOrDefault();

            if (order.Object != null)
            {
                return order.Object;
            }
            else
            {
                throw new Exception("Error loading");
            }
        }

        public async Task<List<AppCore.Models.Order>> GetOrdersForRestaurant(Guid restaurantId)
        {
            var orders = (await firebase.Child("Orders")
                                              .OnceAsync<AppCore.Models.Order>()).Select(item => item.Object).Where(r => r.RestaurantId==restaurantId);

            if (orders != null)
            {
                return new List<AppCore.Models.Order>(orders);
            }
            else
            {
                throw new Exception("Error loading");
            }
        }

        public async Task<bool> Update(AppCore.Models.Order item)
        {
            var restaurantToUpdate = (await firebase
                                     .Child("Orders")
                                     .OnceAsync<AppCore.Models.Order>()).Where(a => a.Object.Id == item.Id).FirstOrDefault();

            if (restaurantToUpdate?.Object == null)
            {
                throw new Exception("Error updating");
            }

            await firebase
              .Child("Orders")
              .Child(restaurantToUpdate.Key)
              .PutAsync(item);

            return true;
        }
    }
}

using Firebase.Database;
using Quick.Order.AppCore.Contracts.Repositories;
using System;
using Firebase.Database.Query;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Reactive.Linq;

namespace Quick.Order.Shared.Infrastructure.Repositories
{
    public class OrdersRepository : IOrdersRepository
    {

        FirebaseClient firebase;

        public OrdersRepository()
        {
            firebase = new FirebaseClient("https://quickorder-f339b-default-rtdb.europe-west1.firebasedatabase.app/");

        }

        public event OrdersEventHandler OrderAddedOrDeleted;
        public event OrdersEventHandler ObservedOrderStatusChanged;

        public async Task<AppCore.Models.Order> Add(AppCore.Models.Order item)
        {
            var result = await firebase.Child("Orders")
                                         .PostAsync(item);


            if (result?.Object == null)
            {
                throw new Exception("Error saving");
            }

            return result.Object;
        }

        public Task<int> Count()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Delete(AppCore.Models.Order item)
        {
            var DeleteUserDb = (await firebase
                                 .Child("Orders")
                                 .OnceAsync<AppCore.Models.Order>()).SingleOrDefault(a => a.Object.Id == item.Id);
            await firebase.Child("Orders").Child(DeleteUserDb.Key).DeleteAsync();

            if (DeleteUserDb?.Object == null)
            {
                throw new Exception("Error deleting");
            }

            return true;
        }

        public async Task<IEnumerable<AppCore.Models.Order>> Get(Func<AppCore.Models.Order,bool> predicate)
        {
            var orders = (await firebase.Child("Orders")
                                                         .OnceAsync<AppCore.Models.Order>()).Select(item => item.Object).Where(predicate);

            if (orders != null)
            {
                return new List<AppCore.Models.Order>(orders);
            }
            else
            {
                throw new Exception("Error loading");
            }
        }

        public async Task<IEnumerable<AppCore.Models.Order>> Get()
        {
            var orders = (await firebase.Child("Orders")
                            .OnceAsync<AppCore.Models.Order>()).Select(item => item.Object);

            if (orders != null)
            {
                return new List<AppCore.Models.Order>(orders);
            }
            else
            {
                throw new Exception("Error loading");
            }
        }

        public async Task<AppCore.Models.Order> GetById(Guid id)
        {
            var order = (await firebase.Child("Orders").OnceAsync<AppCore.Models.Order>()).Where(a => a.Object.Id == id).FirstOrDefault();

            if (order?.Object != null)
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


        public void StartOrdersObservation(Guid restaurantId)
        {
            var observable = firebase
                            .Child("Orders")
                            .AsObservable<AppCore.Models.Order>();

            var subscription = observable.Where(r => r.Object.RestaurantId == restaurantId && r.Object.IsRecent()).Subscribe(n =>
            {
                this.OrderAddedOrDeleted?.Invoke(this, new OrdersEventArgs { IsDeleted = n.EventType == Firebase.Database.Streaming.FirebaseEventType.Delete, Order = n.Object });
            });
        }

        public void StartOrdersStatusObservation(Guid orderId)
        {
            var observable = firebase
                            .Child("Orders")
                            .AsObservable<AppCore.Models.Order>();

            var subscription = observable.Where(r => r.Object.Id == orderId).Subscribe(n =>
            {
                this.ObservedOrderStatusChanged?.Invoke(this, new OrdersEventArgs { IsDeleted = n.EventType == Firebase.Database.Streaming.FirebaseEventType.Delete, Order = n.Object });
            });
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

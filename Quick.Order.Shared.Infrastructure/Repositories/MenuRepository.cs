using Firebase.Database;
using Quick.Order.AppCore.Contracts.Repositories;
using Quick.Order.AppCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database.Query;

namespace Quick.Order.Shared.Infrastructure.Repositories
{
    public class MenuRepository : IMenuRepository
    {
        FirebaseClient firebase;

        public MenuRepository()
        {
            firebase = new FirebaseClient("https://quickorder-f339b-default-rtdb.europe-west1.firebasedatabase.app/");


        }

        public async Task<Menu> Add(Menu item)
        {
            var result = await firebase
                                    .Child("Menus")
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

        public Task<bool> Delete(Menu item)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Restaurant>> Get(Func<Menu, bool> func)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Menu>> Get()
        {
            throw new NotImplementedException();
        }

        public async Task<Menu> GetById(Guid id)
        {
            var restaurants = (await firebase.Child("Menus").OnceAsync<Menu>()).Where(a => a.Object.Restaurant.Id == id).FirstOrDefault();

            if (restaurants.Object != null)
            {
                return restaurants.Object;
            }
            else
            {
                throw new Exception("Error loading");
            }
        }

        public async Task<bool> Update(Menu entityToUpdate)
        {
            var toUpdatePerson = (await firebase
                          .Child("Menus")
                          .OnceAsync<Menu>()).Where(a => a.Object.Restaurant.Id == entityToUpdate.Restaurant.Id).FirstOrDefault();

            if (toUpdatePerson?.Object == null)
            {
                throw new Exception("Error updating");
            }

            await firebase
              .Child(toUpdatePerson.Key)
              .PutAsync(entityToUpdate);

            return true;
        }
    }

}

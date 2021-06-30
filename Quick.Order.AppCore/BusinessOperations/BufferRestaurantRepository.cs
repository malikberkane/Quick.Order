using Quick.Order.AppCore.Contracts.Repositories;
using Quick.Order.AppCore.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Quick.Order.AppCore.BusinessOperations
{
    public class BufferRestaurantRepository : IRestaurantRepository
    {
        private readonly ICacheRestaurantRepository cacheRepository;
        private readonly IRemoteRestaurantRepository remoteRestaurantRepository;

        public BufferRestaurantRepository(ICacheRestaurantRepository cacheRepository, IRemoteRestaurantRepository remoteRestaurantRepository)
        {
            this.cacheRepository = cacheRepository;
            this.remoteRestaurantRepository = remoteRestaurantRepository;
        }
        public async Task<Restaurant> Add(Restaurant item)
        {
            var result = await remoteRestaurantRepository.Add(item);

            if (result != null)
            {
                await cacheRepository.Add(item);
            }


            return null;

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
            var cache = await cacheRepository.Get();

            if (cache != null)
            {
                return cache;
            }
            else
            {
                return await remoteRestaurantRepository.Get();
            }
        }

        public Task<IEnumerable<Restaurant>> Get(Func<Restaurant, bool> func)
        {
            throw new NotImplementedException();
        }

        public Task<Restaurant> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(Restaurant item)
        {
            throw new NotImplementedException();
        }
    }
}

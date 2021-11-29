using Quick.Order.AppCore.Contracts.Repositories;
using Quick.Order.AppCore.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;
using System.Reflection;

namespace Quick.Order.Shared.Infrastructure.Repositories
{
    public class CurrencyRepository : ICurrencyRepository
    {
        public Task<Currency> Add(Currency item)
        {
            throw new NotImplementedException();
        }

        public Task<int> Count()
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(Currency item)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Currency>> Get()
        {
            var result = new List<Currency>();
            var assembly = typeof(CurrencyRepository).GetTypeInfo().Assembly;
            Stream stream = assembly.GetManifestResourceStream("Quick.Order.Shared.Infrastructure.Resources.Common-Currency.json");

            using (var reader = new StreamReader(stream))
            {
                var json = reader.ReadToEnd();
                var rootobject = JsonConvert.DeserializeObject<Rootobject>(json);
                if (rootobject?.currencies != null && rootobject.currencies.Any())
                {
                    result.AddRange(rootobject.currencies);
                }
            }

            return Task.FromResult<IEnumerable<Currency>>(result);
        }

        public Task<Currency> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(Currency item)
        {
            throw new NotImplementedException();
        }
    }

    public class Rootobject
    {
        public List<Currency> currencies { get; set; }
    }
}

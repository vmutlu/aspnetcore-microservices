using Shopping.Aggregator.Extensions;
using Shopping.Aggregator.Models;
using Shopping.Aggregator.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Shopping.Aggregator.Services.Concrete
{
    public class CatalogService : ICatalogService
    {
        private readonly HttpClient _client;

        public CatalogService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<IEnumerable<CatalogModel>> GetCatalog()
        {
            var response = await _client.GetAsync("/api/v1/Catalogs").ConfigureAwait(false);
            return await response.ReadContentAs<List<CatalogModel>>().ConfigureAwait(false);
        }

        public async Task<CatalogModel> GetCatalog(string id)
        {
            var response = await _client.GetAsync($"/api/v1/Catalogs/{id}").ConfigureAwait(false);
            return await response.ReadContentAs<CatalogModel>().ConfigureAwait(false);
        }

        public async Task<IEnumerable<CatalogModel>> GetCatalogByCategory(string category)
        {
            var response = await _client.GetAsync($"/api/v1/Catalogs/GetProductByCategory/{category}").ConfigureAwait(false);
            return await response.ReadContentAs<List<CatalogModel>>().ConfigureAwait(false); 
        }
    }
}

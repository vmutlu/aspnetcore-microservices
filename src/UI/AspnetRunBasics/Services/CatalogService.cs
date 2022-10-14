using AspnetRunBasics.Extensions;
using AspnetRunBasics.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AspnetRunBasics.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly HttpClient _client;

        public CatalogService(HttpClient client, ILogger<CatalogService> logger)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<IEnumerable<CatalogModel>> GetCatalog()
        {
            var response = await _client.GetAsync("/Catalogs").ConfigureAwait(false);
            return await response.ReadContentAs<List<CatalogModel>>().ConfigureAwait(false);
        }

        public async Task<CatalogModel> GetCatalog(string id)
        {
            var response = await _client.GetAsync($"/Catalogs/{id}").ConfigureAwait(false);
            return await response.ReadContentAs<CatalogModel>().ConfigureAwait(false);
        }

        public async Task<IEnumerable<CatalogModel>> GetCatalogByCategory(string category)
        {
            var response = await _client.GetAsync($"/Catalogs/GetProductByCategory/{category}").ConfigureAwait(false);
            return await response.ReadContentAs<List<CatalogModel>>().ConfigureAwait(false);
        }

        public async Task<CatalogModel> CreateCatalog(CatalogModel model)
        {
            var response = await _client.PostAsJson($"/Catalogs", model).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
                return await response.ReadContentAs<CatalogModel>().ConfigureAwait(false);
            else
            {
                throw new Exception("Something went wrong when calling api.");
            }
        }
    }
}

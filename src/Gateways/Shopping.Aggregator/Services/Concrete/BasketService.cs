using Shopping.Aggregator.Models;
using Shopping.Aggregator.Services.Abstract;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using Shopping.Aggregator.Extensions;

namespace Shopping.Aggregator.Services.Concrete
{
    public class BasketService : IBasketService
    {
        private readonly HttpClient _client;

        public BasketService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<BasketModel> GetBasket(string userName)
        {
            var response = await _client.GetAsync($"/api/v1/Baskets/{userName}").ConfigureAwait(false);
            return await response.ReadContentAs<BasketModel>().ConfigureAwait(false);
        }
    }
}

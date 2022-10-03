using Shopping.Aggregator.Models;
using Shopping.Aggregator.Services.Abstract;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using Shopping.Aggregator.Extensions;

namespace Shopping.Aggregator.Services.Concrete
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _client;

        public OrderService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<IEnumerable<OrderResponseModel>> GetOrdersByUserName(string userName)
        {
            var response = await _client.GetAsync($"/api/v1/Orders/{userName}").ConfigureAwait(false);
            return await response.ReadContentAs<List<OrderResponseModel>>().ConfigureAwait(false);
        }
    }
}

using Basket.API.Entities;
using Basket.API.Repositories.Abstract;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Basket.API.Repositories.Concrate
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _cache;

        public BasketRepository(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task DeleteBasket(string userName)
        {
            await _cache.RemoveAsync(userName).ConfigureAwait(false);
        }

        public async Task<ShoppingCart> GetBasket(string userName)
        {
            var basket = await _cache.GetStringAsync(userName).ConfigureAwait(false);
            
            if (string.IsNullOrWhiteSpace(basket)) return null;

            return JsonConvert.DeserializeObject<ShoppingCart>(basket);
        }

        public async Task<ShoppingCart> UpdateBasket(ShoppingCart basket)
        {
            await _cache.SetStringAsync(basket.UserName, JsonConvert.SerializeObject(basket)).ConfigureAwait(false);

            return await GetBasket(basket.UserName).ConfigureAwait(false);
        }
    }
}

using Basket.Api.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Basket.Api.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        #region Constructors

        private readonly IDistributedCache _redisCache;

        public BasketRepository(IDistributedCache redisCache)
        {
            _redisCache = redisCache;
        }

        #endregion

        #region Methods

        public async Task<ShoppingCart> GetUserBasketAsync(string username)
        {
            var basket = await _redisCache
                .GetStringAsync(key: username);

            if (string.IsNullOrEmpty(value: basket))
            {
                return null;
            }

            return JsonConvert.DeserializeObject<ShoppingCart>(value: basket);
        }

        public async Task<ShoppingCart> UpdateBasketAsync(ShoppingCart basket)
        {
            await _redisCache.SetStringAsync
                (
                    basket.UserName,
                    JsonConvert.SerializeObject(value: basket)
                );

            return await GetUserBasketAsync(username: basket.UserName);
        }

        public async Task DeleteBasketAsync(string username)
        {
            await _redisCache.RemoveAsync(key: username);
        }

        #endregion
    }
}

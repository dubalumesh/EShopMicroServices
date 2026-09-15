using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.API.Data
{
    public class CacheBasketRepository(IBasketRepository repository, IDistributedCache cache) : IBasketRepository
    {
        /// <summary>
        /// delete the basket from both the underlying repository and the distributed cache.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken)
        {
            await repository.DeleteBasket(userName, cancellationToken);
            await cache.RemoveAsync(userName, cancellationToken);
            return true;

        }

        /// <summary>
        /// Get the basket from the distributed cache if it exists, otherwise get it from the underlying repository and store it in the cache.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken)
        {
            var cachedBasket = await cache.GetStringAsync(userName, cancellationToken);
            if (cachedBasket == null)
            {
                var basket = await repository.GetBasket(userName, cancellationToken);
                await cache.SetStringAsync(userName, JsonSerializer.Serialize(basket), cancellationToken);
                return basket;
            }
            else
            {
                return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket)!;
            }
        }

        /// <summary>
        /// Store the basket in both the underlying repository and the distributed cache.
        /// </summary>
        /// <param name="basket"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellationToken)
        {
            await repository.StoreBasket(basket, cancellationToken);
            await cache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket), cancellationToken);
            return basket;

        }
    }
}

using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace Equipe.Infrastructure.Cache
{
    public class CacheService(IDistributedCache cache) : ICacheService
    {
        private readonly IDistributedCache _cache = cache;

        public async Task<T?> GetAsync<T>(string key)
        {
            var cachedData = await _cache.GetStringAsync(key);
            return cachedData is null ? default : JsonSerializer.Deserialize<T>(cachedData);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(5)
            };

            var jsonData = JsonSerializer.Serialize(value);
            await _cache.SetStringAsync(key, jsonData, options);
        }

        public async Task RemoveAsync(string key)
        {
            await _cache.RemoveAsync(key);
        }
    }
}
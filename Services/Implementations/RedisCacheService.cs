namespace PremierLeague_Api.Services.Implementations
{
    using Microsoft.Extensions.Caching.Distributed;
    using PremierLeague_Api.Services.Interfaces;
    using System.Text.Json;

    public class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache cache;

        private static readonly JsonSerializerOptions jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public RedisCacheService(IDistributedCache cache)
        {
            this.cache = cache;
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var cachedData = await cache.GetStringAsync(key);

            if (string.IsNullOrWhiteSpace(cachedData))
                return default;

            return JsonSerializer.Deserialize<T>(cachedData, jsonOptions);
        }

        public async Task SetAsync<T>(string key, T value, int minutes)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(minutes),

                SlidingExpiration = TimeSpan.FromMinutes(1)
            };

            await cache.SetStringAsync(key, JsonSerializer.Serialize(value, jsonOptions), options);
        }

        public async Task RemoveAsync(string key)
        {
            await cache.RemoveAsync(key);
        }

        public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> getData, int minutes)
        {
            var cachedData = await GetAsync<T>(key);

            if (cachedData != null)
                return cachedData;

            var data = await getData();

            if (data != null)
            {
                await SetAsync(key, data, minutes);
            }

            return data;
        }
    }
}

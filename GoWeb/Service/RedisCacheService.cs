using GoWeb.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System.Text.Json;

namespace GoWeb.Service
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache _cache;
        private readonly IConnectionMultiplexer multiplexerCache;

        public RedisCacheService(IDistributedCache cache, IConnectionMultiplexer multiplexerCache)
        {
            _cache = cache;
            this.multiplexerCache = multiplexerCache;
        }

        public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            var cachedString = await _cache.GetStringAsync(key, cancellationToken);

            if (string.IsNullOrEmpty(cachedString))
                return default;

            return JsonSerializer.Deserialize<T>(cachedString);
        }

        public async Task SetAsync<T>(string key, T value, DistributedCacheEntryOptions? timeOption = null, CancellationToken cancellationToken = default)
        {

            var options = new DistributedCacheEntryOptions();
            if (timeOption!=null)
            {
                options= timeOption;
            }
            var json = JsonSerializer.Serialize(value);
            await _cache.SetStringAsync(key, json, options, cancellationToken);
        }

        public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            await _cache.RemoveAsync(key, cancellationToken);
        }

        public async Task<Dictionary<string, T>> GetManyAsync<T>(IEnumerable<string> keys)
        {
            var db = multiplexerCache.GetDatabase();
            var redisKeys = keys.Select(k => (RedisKey)k).ToArray();
            RedisValue[] values = await db.StringGetAsync(redisKeys);
            var result = new Dictionary<string, T>();
            for (int i = 0; i < redisKeys.Length; i++)
            {
                if (values[i].HasValue)
                {
                    var stringValue = values[i].ToString();
                    var deserializedValue = JsonSerializer.Deserialize<T>(stringValue);
                    if (deserializedValue != null)
                    {
                        result.Add(keys.ElementAt(i), deserializedValue);
                    }
                }
            }
            return result;
        }

    }
}


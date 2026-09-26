using GoWeb.Shared.Models;
using Microsoft.Extensions.Caching.Distributed;

namespace GoWeb.Interfaces
{
    public interface ICacheService
    {
        Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default);

        Task SetAsync<T>(string key, T value,  DistributedCacheEntryOptions? timeOpion = null, CancellationToken cancellationToken = default);

        Task RemoveAsync(string key, CancellationToken cancellationToken = default);

        Task<Dictionary<string, T>> GetManyAsync<T>(IEnumerable<string> keys);

        Task<(bool IsSuccess, T? Value)> TryGetValueAsync<T>(string key, CancellationToken cancellationToken = default);
    }
}

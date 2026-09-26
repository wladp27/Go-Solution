using GoWeb.Interfaces;
using GoWeb.Сonstants.Cache;
using GoWebApplication.Db.Models;
using System.Collections.Concurrent;

namespace GoWeb.Service
{
    public class StatusEventService : IStatusEventService
    {
        private readonly ICacheService _cache;
        private readonly IStatusEvent _statusEventRepository;

        private static readonly SemaphoreSlim _semForGetAll = new SemaphoreSlim(1, 1);
        private static readonly ConcurrentDictionary<int, SemaphoreSlim> _semaphoresById = new();

        public StatusEventService(ICacheService cache, IStatusEvent statusEvent)
        {
            _cache = cache;
            _statusEventRepository = statusEvent;
        }

        public async Task<List<StatusEvent>> GetAllAsync()
        {
            var (isSuccess, statusesEvent) = await _cache.TryGetValueAsync<List<StatusEvent>>(CacheConst.allStatusesEvent);
            if (isSuccess)
            {
                return statusesEvent ?? new();
            }

            await _semForGetAll.WaitAsync();
            try
            {
                (isSuccess, statusesEvent) = await _cache.TryGetValueAsync<List<StatusEvent>>(CacheConst.allStatusesEvent);
                if (isSuccess)
                {
                    return statusesEvent ?? new();
                }

   
                statusesEvent = await _statusEventRepository.GetAllAsync();
                await _cache.SetAsync(CacheConst.allStatusesEvent, statusesEvent);
            }
            finally
            {
                _semForGetAll.Release();
            }

            return statusesEvent ?? new();
        }

        public async Task<StatusEvent?> GetByIdAsync(int id)
        {
            string cacheKey = new StatusEventCacheKey(id).ToString();

            var (isSuccess, statusEvent) = await _cache.TryGetValueAsync<StatusEvent>(cacheKey);
            if (isSuccess)
            {
                return statusEvent;
            }
            var semaphore = _semaphoresById.GetOrAdd(id, _ => new SemaphoreSlim(1, 1));
            await semaphore.WaitAsync();
            try
            {
                (isSuccess, statusEvent) = await _cache.TryGetValueAsync<StatusEvent>(cacheKey);
                if (isSuccess)
                {
                    return statusEvent;
                }

                statusEvent = await _statusEventRepository.GetByIdAsync(id);
                await _cache.SetAsync(cacheKey, statusEvent);
            }
            finally
            {
                semaphore.Release();
            }

            return statusEvent;
        }
    }

    public record StatusEventCacheKey(int id)
    {
        public override string ToString() => $"event:status:{id}";
    }
}
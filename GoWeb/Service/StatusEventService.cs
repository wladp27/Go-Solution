using GoWeb.Interfaces;
using GoWeb.Сonstants.Cache;
using GoWebApplication.Db.Models;
using Microsoft.Extensions.Caching.Memory;

namespace GoWeb.Service
{
    public class StatusEventService : IStatusEventService
    {
        private readonly ICacheService cache;
        private readonly IStatusEvent statusEventRepository;

        private static readonly SemaphoreSlim semForGetAll = new SemaphoreSlim(1, 1);
        private static readonly SemaphoreSlim semForGetId = new SemaphoreSlim(1, 1);

        public StatusEventService(ICacheService cache, IStatusEvent statusEvent) 
        {
            this.cache = cache;
            this.statusEventRepository = statusEvent;
        }

        public async Task<List<StatusEvent>> GetAllAsync()
        {
            var resultCache = await cache.TryGetValueAsync<List<StatusEvent>>(CacheConst.allStatusesEvent);
            var statusesEvent = resultCache.Value;
            if (resultCache.IsSuccess)
            {
                return statusesEvent ?? new();
            }
            await semForGetAll.WaitAsync();
            try
            {
                resultCache = await cache.TryGetValueAsync<List<StatusEvent>>(CacheConst.allStatusesEvent);
                statusesEvent = resultCache.Value;
                if (resultCache.IsSuccess)
                {
                    return statusesEvent ?? new();
                }
                statusesEvent = await statusEventRepository.GetAllAsync();
                await cache.SetAsync(CacheConst.allStatusesEvent, statusesEvent);
            }
            finally
            {
                semForGetAll.Release();
            }
            return statusesEvent;
        }

        public async Task<StatusEvent?> GetByIdAsync(int id)
        {
            var resultCache = await cache.TryGetValueAsync<StatusEvent>(new StatusEventCacheKey(id).ToString());
            var statusEvent = resultCache.Value;
            if (resultCache.IsSuccess)
            {
                return statusEvent;
            }
            await semForGetId.WaitAsync();
            try
            {
                resultCache = await cache.TryGetValueAsync<StatusEvent>(new StatusEventCacheKey(id).ToString());
                statusEvent = resultCache.Value;
                if (resultCache.IsSuccess)
                {
                    return statusEvent;
                }
                statusEvent = await statusEventRepository.GetByIdAsync(id);
                await cache.SetAsync(new StatusEventCacheKey(id).ToString(), statusEvent);
            }
            finally
            {
                semForGetId.Release();
            }
            return statusEvent;
        }

       
    }

    public record StatusEventCacheKey(int id)
    {
        public override string ToString() => $"event:status:{id}";
    }
}

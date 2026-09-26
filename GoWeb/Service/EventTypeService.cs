using AutoMapper;
using GoWeb.Interfaces;
using GoWeb.Models;
using GoWeb.Shared.Models;
using GoWeb.Сonstants.Cache;
using GoWebApplication.Db.Models;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;

namespace GoWeb.Service
{
    public class EventTypeService : IEventTypeService
    {
        private readonly IEventTypeRepository eventTypeRepository;
        private readonly ICacheService cache;
        private static readonly SemaphoreSlim semForGetAll = new SemaphoreSlim(1, 1);
        private static readonly ConcurrentDictionary<int, SemaphoreSlim> _semaphoresById = new();
        private readonly IMapper mapper;
        public EventTypeService(IEventTypeRepository eventTypeRepository, ICacheService cache, IMapper mapper) 
        {
            this.eventTypeRepository = eventTypeRepository;
            this.cache = cache;
            this.mapper = mapper;
        }
        public async Task<bool> AddAsync(EventTypeDTO eventTypeView)
        {
            eventTypeView.Id = await eventTypeRepository.AddAsync(mapper.Map<EventType>(eventTypeView));
            if (eventTypeView.Id >0)
            {
                await cache.SetAsync(new EventTypeCacheKey(eventTypeView.Id).ToString(),eventTypeView);
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteAsync(EventTypeDTO eventType)
        {
            var successDelete= await eventTypeRepository.DeleteAsync(mapper.Map<EventType>(eventType));
            if(successDelete)
            {
                await cache.RemoveAsync(new EventTypeCacheKey(eventType.Id).ToString());
                return true;
            }
            return false;
        }

        public async Task<List<EventTypeDTO>?> GetAllAsync()
        {
            var (isSuccess, eventsTypes) = await cache.TryGetValueAsync<List<EventTypeDTO>>(CacheConst.allEventTypes);
            if (isSuccess)
            {
                return eventsTypes;
            }
            await semForGetAll.WaitAsync();
            try
            {
                (isSuccess, eventsTypes) = await cache.TryGetValueAsync<List<EventTypeDTO>>(CacheConst.allEventTypes);
                if (isSuccess)
                {
                    return eventsTypes;
                }
                var eventsTypesDb = await eventTypeRepository.GetAllAsync();
                eventsTypes = mapper.Map<List<EventTypeDTO>>(eventsTypesDb);
                await cache.SetAsync(CacheConst.allEventTypes, eventsTypes, new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromDays(1)));
            }
            finally
            {
                semForGetAll.Release();
            }
            return eventsTypes;
        }

        public async Task<EventTypeDTO?> GetByIdAsync(int id)
        {
            var (isSuccess, eventTypeView) = await cache.TryGetValueAsync<EventTypeDTO>(new EventTypeCacheKey(id).ToString());
            if (isSuccess)
            {
                return eventTypeView;
            }

            var semaphore = _semaphoresById.GetOrAdd(id, _ => new SemaphoreSlim(1, 1));
            await semaphore.WaitAsync();
            try
            {
                (isSuccess, eventTypeView) = await cache.TryGetValueAsync<EventTypeDTO>(new EventTypeCacheKey(id).ToString());
                if (isSuccess)
                {
                    return eventTypeView;
                }
                var eventTypeDb = await eventTypeRepository.GetByIdAsync(id);
                eventTypeView = mapper.Map<EventTypeDTO>(eventTypeDb);
                if (eventTypeDb!=null)
                {
                    await cache.SetAsync(new EventTypeCacheKey(id).ToString(), eventTypeView);
                }
                else
                {
                    await cache.SetAsync(new EventTypeCacheKey(id).ToString(), eventTypeView, new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(1)));
                }
            }
            finally 
            {
                semaphore.Release(); 
            }
            return eventTypeView;
        }

        public async Task<bool> Update(EventTypeDTO eventTypeView)
        {
            var updateEventDb= mapper.Map<EventType>(eventTypeView);
            var successUpdate = await eventTypeRepository.Update(updateEventDb);
            if(successUpdate)
            {
                await cache.SetAsync(new EventTypeCacheKey(eventTypeView.Id).ToString(), eventTypeView);
            }
            return false;
        }

        public record EventTypeCacheKey(int id)
        {
            public override string ToString() => $"EventType:summary:{id}";
        }

    }
}

using AutoMapper;
using GoWeb.Interfaces;
using GoWeb.Models;
using GoWeb.Shared.Models;
using GoWeb.Сonstants.Cache;
using GoWebApplication.Db.Models;
using Microsoft.Extensions.Caching.Memory;

namespace GoWeb.Service
{
    public class EventTypeService : IEventTypeService
    {
        private readonly IEventTypeRepository eventTypeRepository;
        private readonly IMemoryCache cache;
        private static readonly SemaphoreSlim semForGetAll = new SemaphoreSlim(1, 1);
        private static readonly SemaphoreSlim semForGetId = new SemaphoreSlim(1, 1);
        private readonly IMapper mapper;
        public EventTypeService(IEventTypeRepository eventTypeRepository, IMemoryCache cache, IMapper mapper) 
        {
            this.eventTypeRepository = eventTypeRepository;
            this.cache = cache;
            this.mapper = mapper;
        }
        public async Task<bool> AddAsync(EventTypeDTO eventTypeView)
        {
            eventTypeView.Id = await eventTypeRepository.AddAsync(mapper.Map<EventType>(eventTypeView));
            if (eventTypeView.Id != null)
            {
                cache.Set(new EventTypeCacheKey(eventTypeView.Id),eventTypeView);
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteAsync(EventTypeDTO eventType)
        {
            var successDelete= await eventTypeRepository.DeleteAsync(mapper.Map<EventType>(eventType));
            if(successDelete)
            {
                cache.Remove(new EventTypeCacheKey(eventType.Id));
                return true;
            }
            return false;
        }

        public async Task<List<EventTypeDTO>?> GetAllAsync()
        {
            var eventsTypes= new List<EventTypeDTO>();
            if(cache.TryGetValue(CacheConst.allEventTypes, out eventsTypes))
            {
                return eventsTypes;
            }
            await semForGetAll.WaitAsync();
            try
            {
                if (cache.TryGetValue(CacheConst.allEventTypes, out eventsTypes))
                {
                    return eventsTypes;
                }
                var eventsTypesDb = await eventTypeRepository.GetAllAsync();
                eventsTypes = mapper.Map<List<EventTypeDTO>>(eventsTypesDb);
                cache.Set(CacheConst.allEventTypes, eventsTypes, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromDays(1)));
            }
            finally
            {
                semForGetAll.Release();
            }
            return eventsTypes;
        }

        public async Task<EventTypeDTO?> GetByIdAsync(int id)
        {
            if(cache.TryGetValue(new EventTypeCacheKey(id), out EventTypeDTO eventTypeView))
            {
                return eventTypeView;
            }
            await semForGetId.WaitAsync();
            try 
            {
                if (cache.TryGetValue(new EventTypeCacheKey(id), out  eventTypeView))
                {
                    return eventTypeView;
                }
                var eventTypeDb = eventTypeRepository.GetByIdAsync(id);
                eventTypeView = mapper.Map<EventTypeDTO>(eventTypeDb);
                if (eventTypeDb!=null)
                {
                    cache.Set(new EventTypeCacheKey(id), eventTypeView);
                }
                else
                {
                    cache.Set(new EventTypeCacheKey(id), eventTypeView, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(1)));
                }
            }
            finally 
            {
                semForGetId.Release(); 
            }
            return eventTypeView;
        }

        public async Task<bool> Update(EventTypeDTO eventTypeView)
        {
            var updateEventDb= mapper.Map<EventType>(eventTypeView);
            var successUpdate = await eventTypeRepository.Update(updateEventDb);
            if(successUpdate)
            {
                cache.Set(new EventTypeCacheKey(eventTypeView.Id), eventTypeView);
            }
            return false;
        }

        public record EventTypeCacheKey(int id);

    }
}

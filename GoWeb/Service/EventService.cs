using AutoMapper;
using AutoMapper.QueryableExtensions;
using GoWeb.Commands.Event;
using GoWeb.Interfaces;
using GoWeb.Models;
using GoWeb.Shared.Model;
using GoWeb.Shared.Models;
using GoWeb.Сonstants;
using GoWebApplication.Db.Data;
using GoWebApplication.Db.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Concurrent;

namespace GoWeb.Service
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IEventTypeService _eventTypeService;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ICacheService _cache; 
        private readonly ILocationRepository _locationRepository;
        private readonly ILogger<CheckingСancelEventHandler> _logger;
        private readonly ICityService _cityService;
        private static readonly ConcurrentDictionary<int, SemaphoreSlim> _semaphoresById = new();
        private static readonly ConcurrentDictionary<string, SemaphoreSlim> _semaphoresByFilter = new();

        public EventService(
            IEventRepository eventRepository,
            IMapper mapper,
            ICacheService cache,
            ICityService cityService,
            IEventTypeService eventTypeService,
            ILocationRepository locationRepository,
            ILogger<CheckingСancelEventHandler> logger,
            IUserRepository userRepository)
        {
            _eventRepository = eventRepository;
            _mapper = mapper;
            _cache = cache;
            _locationRepository = locationRepository;
            _logger = logger;
            _cityService = cityService;
            _eventTypeService = eventTypeService;
            _userRepository = userRepository;
        }

        public async Task<List<EventTypeDTO>?> GetTypesEventsForCity(int idCity)
        {
            return await _eventRepository.GetAllEventsQueryable()
                .Where(ev => ev.Location.CityId == idCity && ev.StatusEventId == (int)StatusEventConts.Published)
                .Select(ev => ev.EventType)
                .Distinct()
                .ProjectTo<EventTypeDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<EventIndexViewModel> GetFilterEvents(int? selectedCity, int? selectedTypeEvent)
        {
            var filter = await GetDataForFilter();
            filter.SelectedCity = selectedCity;
            filter.SelectedTypeEvent = selectedTypeEvent;

            return new EventIndexViewModel { Filter = filter };
        }

        public async Task<EventFilterDTO> GetDataForFilter()
        {
            var listCity = await _cityService.GetAllAsync();
            var listTypeEvents = await _eventTypeService.GetAllAsync();
            return new EventFilterDTO
            {
                Cities = listCity,
                TypeEvents = listTypeEvents,
            };
        }

        public async Task<List<CommandViewModel>> GetCommandChekingCanckeledEventAsync()
        {
            var listEventDb = await _eventRepository.GetAllEventsQueryable()
                .Include(e => e.Location)
                .Where(e => e.StatusEventId == (int)StatusEventConts.Published)
                .ToListAsync();

            return listEventDb.Select(e => new CommandViewModel
            {
                command = new CheckingСancelEventCommand(e.Id, e.EndTime),
                StartTime = e.EndTime.AddHours(-(double)Timings.CanceledTime)
            }).ToList();
        }

        public async Task<List<CommandViewModel>> GetCommandRecreateEventAsync()
        {
            var listEventDb = await _eventRepository.GetAllEventsQueryable()
                .Include(e => e.Location)
                .Where(e => e.StatusEventId == (int)StatusEventConts.ReСreation)
                .ToListAsync();

            return listEventDb.Select(e => new CommandViewModel
            {
                command = new RecreateEventCommand(e.Id),
                StartTime = e.EndTime.AddHours((double)Timings.RecreateTime)
            }).ToList();
        }

        public async Task<List<EventSummaryDTO>?> GetFilteredEventsAsync(EventFilterDTO filter)
        {
            if (filter.SelectedCity == null || filter.SelectedCity == 0) return null;
            string filterKey = new EventFilterCacheKey(filter.SelectedCity, filter.SelectedTypeEvent).ToString();
            var (isSuccess, listIdEvents) = await _cache.TryGetValueAsync<List<int>>(filterKey);
            if (isSuccess && listIdEvents != null)
            {
                return await GetEventsCacheAndDbAsync(listIdEvents);
            }
            var semaphore = _semaphoresByFilter.GetOrAdd(filterKey, _ => new SemaphoreSlim(1, 1));
            await semaphore.WaitAsync();
            try
            {
                (isSuccess, listIdEvents) = await _cache.TryGetValueAsync<List<int>>(filterKey);
                if (isSuccess && listIdEvents != null)
                {
                    return await GetEventsCacheAndDbAsync(listIdEvents);
                }

                var listIdEventsDB = await FilterEventsView(filter, _eventRepository.GetAllEventsQueryable());

                if (listIdEventsDB != null && listIdEventsDB.Any())
                {
                    await _cache.SetAsync(filterKey, listIdEventsDB, new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(60) });
                    return await GetEventsCacheAndDbAsync(listIdEventsDB);
                }


                await _cache.SetAsync(filterKey, new List<int>(), new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1) });
                return new List<EventSummaryDTO>();
            }
            finally
            {
                semaphore.Release();
            }
        }

        public async Task<int> AddAsync(Event ev)
        {
            var idEvent = await _eventRepository.AddAsync(ev);
            if (idEvent > 0 && ev.StatusEventId == (int)StatusEventConts.Published)
            {
                var eventView = _mapper.Map<EventSummaryDTO>(ev);
                if (ev.Location == null && ev.LocationId.HasValue)
                    eventView.Location = _mapper.Map<LocationCreateViewModel>(await _locationRepository.GetByIdAsync(ev.LocationId.Value));

                var timeLive = ev.EndTime - DateTimeOffset.Now;
                if (timeLive > TimeSpan.Zero)
                {
                    await _cache.SetAsync(new EventSummaryCacheKey(eventView.Id).ToString(), eventView, new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = timeLive });
                }
                await RemoveCacheFiltersAsync(eventView);
            }
            return idEvent;
        }

        public async Task<int> AddAsync(EventDTO ev)
        {
            var eventDB = _mapper.Map<Event>(ev);
            var idEvent = await _eventRepository.AddAsync(eventDB);
            if (idEvent > 0)
            {
                await UpdateCacheAsync(eventDB);
            }
            return idEvent;
        }

        public async Task UpdateAsync(EventDTO ev)
        {
            var eventDB = _mapper.Map<Event>(ev);
            await _eventRepository.Update(eventDB);

            // Удаляем старый кэш и обновляем его
            await _cache.RemoveAsync(new EventSummaryCacheKey(eventDB.Id).ToString());
            await UpdateCacheAsync(eventDB);
        }

        public async Task UpdateCacheAsync(Event eventDB)
        {
            if (eventDB.StatusEventId == (int)StatusEventConts.Published)
            {
                var eventView = _mapper.Map<EventSummaryDTO>(eventDB);
                if (eventDB.LocationId.HasValue)
                    eventView.Location = _mapper.Map<LocationCreateViewModel>(await _locationRepository.GetByIdAsync(eventDB.LocationId.Value));

                var timeLive = eventDB.EndTime - DateTimeOffset.Now;
                if (timeLive > TimeSpan.Zero)
                {
                    await _cache.SetAsync(new EventSummaryCacheKey(eventView.Id).ToString(), eventView, new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = timeLive });
                }
                await RemoveCacheFiltersAsync(eventView);
            }
        }

        public async Task<EventSummaryDTO?> GetPublichEventByIdAsync(int id)
        {
            string cacheKey = new EventSummaryCacheKey(id).ToString();

            var (isSuccess, ev) = await _cache.TryGetValueAsync<EventSummaryDTO>(cacheKey);
            if (isSuccess) return ev;

            var semaphore = _semaphoresById.GetOrAdd(id, _ => new SemaphoreSlim(1, 1));
            await semaphore.WaitAsync();
            try
            {
                (isSuccess, ev) = await _cache.TryGetValueAsync<EventSummaryDTO>(cacheKey);
                if (isSuccess) return ev;

                var evDb = await _eventRepository.GetAllEventsQueryable()
                    .Where(e => id == e.Id && e.StatusEventId == (int)StatusEventConts.Published)
                    .ProjectTo<EventSummaryDTO>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();

                if (evDb != null)
                {
                    var timeLive = evDb.EndTime - DateTimeOffset.Now;
                    // Если событие уже в прошлом, храним недолго
                    TimeSpan expireTime = timeLive > TimeSpan.Zero ? timeLive : TimeSpan.FromMinutes(10);
                    await _cache.SetAsync(cacheKey, evDb, new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = expireTime});
                }
                else
                {
                    // Кэшируем отсутствие, чтобы не долбить БД
                    await _cache.SetAsync(cacheKey, (EventSummaryDTO?)null, new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1) });
                }

                return evDb;
            }
            finally
            {
                semaphore.Release();
            }
        }

        public async Task<List<int>> FilterEventsView(EventFilterDTO filter, IQueryable<Event> queryableEvent)
        {
            if (filter.SelectedCity != null && filter.SelectedCity != 0)
            {
                queryableEvent = queryableEvent.Where(e => e.Location.CityId == filter.SelectedCity.Value);
            }

            if (filter.SelectedTypeEvent != null && filter.SelectedTypeEvent != 0)
            {
                queryableEvent = queryableEvent.Where(e => e.EventTypeId == filter.SelectedTypeEvent.Value);
            }

            return await queryableEvent
                .Where(e => e.StatusEventId == (int)StatusEventConts.Published)
                .Select(e => e.Id)
                .ToListAsync();
        }

        public async Task RemoveCacheFiltersAsync(EventSummaryDTO eventSummary)
        {
            // Сбрасываем кэш фильтра только по городу
            string filterCity = new EventFilterCacheKey(eventSummary.Location.CityId, null).ToString();
            await _cache.RemoveAsync(filterCity);

            // Сбрасываем кэш фильтра по городу + типу события
            string filterCityTypeEvent = new EventFilterCacheKey(eventSummary.Location.CityId, eventSummary.EventTypeId).ToString();
            await _cache.RemoveAsync(filterCityTypeEvent);
        }

        private async Task<List<EventSummaryDTO>> GetPublishedEventsDbAsync(List<int> idEventsNotInCache)
        {
            if (!idEventsNotInCache.Any()) return new List<EventSummaryDTO>();

            return await _eventRepository.GetAllEventsQueryable()
                .Where(e => idEventsNotInCache.Contains(e.Id) && e.StatusEventId == (int)StatusEventConts.Published)
                .ProjectTo<EventSummaryDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        private async Task WriteEventsInCacheAsync(List<EventSummaryDTO> listEvents)
        {
            foreach (var evView in listEvents)
            {
                var timeLive = evView.EndTime - DateTimeOffset.Now;
                if (timeLive > TimeSpan.Zero)
                {
                    await _cache.SetAsync(new EventSummaryCacheKey(evView.Id).ToString(), evView,
                        new DistributedCacheEntryOptions{ AbsoluteExpirationRelativeToNow = timeLive });
                    _logger.LogInformation("Cобытие с id:{id} добавлено в кеш", evView.Id);
                }
            }
        }

        // Переписанный метод для массового получения из кэша (N+1 исправлено)
        private async Task<List<EventSummaryDTO>> GetEventsCacheAndDbAsync(List<int>? listIdEventsDB)
        {
            if (listIdEventsDB == null || !listIdEventsDB.Any()) return new List<EventSummaryDTO>();

            var listEventsView = new List<EventSummaryDTO>();
            var idEventsNotInCache = new List<int>();

            // Собираем список строковых ключей
            var keys = listIdEventsDB.Select(id => new EventSummaryCacheKey(id).ToString()).ToList();

            // Одним запросом забираем из Redis всё, что есть
            var dictInCache = await _cache.GetManyAsync<EventSummaryDTO>(keys);

            foreach (var id in listIdEventsDB)
            {
                string key = new EventSummaryCacheKey(id).ToString();

                if (dictInCache.TryGetValue(key, out var evView) && evView != null)
                {
                    listEventsView.Add(evView);
                }
                else
                {
                    idEventsNotInCache.Add(id);
                }
            }

            // Если кого-то не нашли - идем в базу
            if (idEventsNotInCache.Count > 0)
            {
                var eventsNotInCache = await GetPublishedEventsDbAsync(idEventsNotInCache);
                listEventsView.AddRange(eventsNotInCache);
                await WriteEventsInCacheAsync(eventsNotInCache);
            }

            return listEventsView;
        }

        public async Task<bool> ExistenceEvent(int idEvent) => await _eventRepository.ExistenceEvent(idEvent);

        public async Task<bool> UpdateStatusEvent(int idEvent, StatusEventConts status)
        {
            if (status != StatusEventConts.Published && status != StatusEventConts.ReСreation)
            {
                await _cache.RemoveAsync(new EventSummaryCacheKey(idEvent).ToString());
                _logger.LogInformation("Cобытие с {id} удалено из кеша после обновления статуса на {status}", idEvent, Enum.GetName(status));
            }
            return await _eventRepository.UpdateStatusEvent(idEvent, status);
        }

        public async Task<bool> CheckingCountUserAndStatus(int idEvent) => await _eventRepository.CheckingCountUserAndStatus(idEvent);

        public async Task<bool> DeleteBuIdAsync(int idEvent)
        {
            await _cache.RemoveAsync(new EventSummaryCacheKey(idEvent).ToString());
            return await _eventRepository.DeleteBuIdAsync(idEvent);
        }

        public async Task<EventSummaryDTO?> GetEventByIdAsync(int id)
        {
            return await _eventRepository.GetAllEventsQueryable()
                .Where(e => id == e.Id)
                .ProjectTo<EventSummaryDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }
    }

    public record EventSummaryCacheKey(int idEvent)
    {
        public override string ToString() => $"event:summary:{idEvent}";
    }
    public record EventFilterCacheKey(int? cityId, int? typeEventId)
    {
        public override string ToString() => $"event:filter:city:{cityId ?? 0}:type:{typeEventId ?? 0}";
    }
}
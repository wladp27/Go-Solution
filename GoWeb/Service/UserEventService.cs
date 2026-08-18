using AutoMapper;
using GoWeb.Interfaces;
using GoWeb.Shared.Models;
using GoWeb.Shared.Сonstants;
using GoWebApplication.Db.Models;
using Microsoft.Extensions.Caching.Memory;

namespace GoWeb.Service
{
    public class UserEventService : IUserEventService
    {
        private readonly IUserEvent userEventRepository;
        private readonly IUserService userService;
        private readonly IMemoryCache cache;
        private readonly IEventService eventService;
        private readonly IEventRepository eventRepository;
        private readonly IRatingRepository ratingRepository;
        private readonly IMapper mapper;


        private static readonly SemaphoreSlim semofor = new SemaphoreSlim(1, 1);
        public UserEventService(IRatingRepository ratingRepository,IUserEvent userEventService, IUserService userService, IMemoryCache cache, IUserRepository userRepository, IEventService eventService, IEventRepository eventRepository, IMapper mapper) 
        {
            this.eventRepository = eventRepository;
            this.mapper = mapper;
            this.eventService = eventService;
            this.userEventRepository = userEventRepository;
            this.userEventRepository= userEventService;
            this.userService= userService;
            this.cache= cache;
            this.ratingRepository = ratingRepository;
        }
        public async Task<List<Event>> GetAllAttendedEventsUserAsync(string idUser)
        {
           return await userEventRepository.GetAllAttendedEventsUserAsync(idUser);
        }

        public async Task<List<User>> GetRegisteredUsersAsync(int idEvent)
        {
            return await userEventRepository.GetRegisteredUsersAsync(idEvent);
        }

        public async Task<List<UserEvent>> GetUsersRegistAndReservAsync(int idEvent)
        {
            return await userEventRepository.GetUsersRegistAndReservAsync(idEvent);
        }

        public async Task<Dictionary<JoiningStatus, List<User>>> GetUsersRegistAndReservDictionaryAsync(int idEvent)
        {
           return await userEventRepository.GetUsersRegistAndReservDictionaryAsync(idEvent);
        }

        public async Task<JoinResult> JoinAsync(string idUser, int idEvent)
        {
            var result = await userEventRepository.JoinAsync(idUser, idEvent);
            var succesResult = new JoinResult[] { JoinResult.SuccessNewRegistration, JoinResult.SuccessInReserve, JoinResult.SuccessStatusUpdated };
            if(succesResult.Contains(result))
            {
                cache.Remove(new UsersInEventCacheKey(idEvent));
                cache.Remove(new EventCacheKey(idEvent));
            }
            return result;  
        }

        public async Task<LeaveResult> LeaveUserAsync(string idUser, int idEvent)
        {
            var result = await userEventRepository.LeaveUserAsync(idUser, idEvent);
            if (result==LeaveResult.SuccessLeave)
            {
                cache.Remove(new UsersInEventCacheKey(idEvent));
                cache.Remove(new EventCacheKey(idEvent));
            }
            return result;
        }


        public async Task<List<UserPrewievDTO>?> GetUsersEventAsync(int idEvent)
        {
            var cacheKey = new UsersInEventCacheKey(idEvent);
            if (cache.TryGetValue(cacheKey, out List<string>? idUsers))
            {
                if (idUsers == null)
                    return null;
                if (idUsers.Count == 0)
                    return new(); 
                return await userService.GetPreviewUsers(idUsers);
            }

            await semofor.WaitAsync();
            try
            {
                if (cache.TryGetValue(cacheKey, out idUsers))
                {
                    if (idUsers == null)
                        return null;

                    if (idUsers.Count == 0)
                        return new();

                    return await userService.GetPreviewUsers(idUsers);
                }
                var listIdUsersInEvent = await userService.GetIdUsersDB(idEvent);
                if (listIdUsersInEvent == null)
                {
                    cache.Set(cacheKey, listIdUsersInEvent , TimeSpan.FromMinutes(5));
                    return null;
                }
                cache.Set(cacheKey, listIdUsersInEvent, TimeSpan.FromMinutes(60));
                if (listIdUsersInEvent.Count == 0)
                    return new();
                return await userService.GetPreviewUsers(listIdUsersInEvent!);
            }
            finally
            {
                semofor.Release();
            }
        }


        /// <summary>
        /// Данный метод использует два запроса из-за дублирования строк события на каждую строку пользователя
        /// </summary>
        public async Task<EventWithUsersDTO?> GetEventsWithUserAsync(int idEvent)
        {
            var ev = await eventService.GetPublichEventByIdAsync(idEvent);
            if(ev == null)
                return null;
            var evView = mapper.Map<EventWithUsersDTO>(ev);
            var users = await GetUsersEventAsync(idEvent) ?? new();
            evView.Users = users;
            return evView;
        }


        public record UsersInEventCacheKey(int idEvent);
    }
}

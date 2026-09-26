using System.Collections.Concurrent;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using GoWeb.Interfaces;
using GoWeb.Shared.Models;
using GoWeb.Shared.Сonstants;
using GoWebApplication.Db.Data;
using GoWebApplication.Db.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace GoWeb.Service
{
    public class UserService : IUserService
    {
        private readonly ICacheService _cache;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        private static readonly ConcurrentDictionary<string, SemaphoreSlim> _semaphoresById = new();

        public UserService(ICacheService cache, IMapper mapper, IUserRepository userRepository, ApplicationDbContext context)
        {
            _cache = cache;
            _mapper = mapper;
            _userRepository = userRepository;
            _context = context;
        }

        //  Перенести работу с _context в userRepository
        public async Task<List<string>> GetIdUsersDB(int idEvent)
        {
            return await _context.UsersEvents
                .Where(ue => ue.EventId == idEvent &&
                            (ue.StatusJoiningId == (int)JoiningStatus.Registered || ue.StatusJoiningId == (int)JoiningStatus.InReserve))
                .OrderBy(ue => ue.StatusJoiningId)
                .ThenBy(ue => ue.TimeJoinEvent)
                .Select(ue => ue.User.Id)
                .ToListAsync();
        }

        public async Task<List<UserPrewievDTO>> GetPreviewUsers(List<string> idUsers)
        {
            var listUserPreview = new List<UserPrewievDTO>();
            var idUserNotInCache = new List<string>();
            var listKey = idUsers.Select(id => new UsersPreviewCacheKey(id).ToString()).ToList();

            var dictionarUserInCache = await _cache.GetManyAsync<UserPrewievDTO>(listKey);

            foreach (var id in idUsers)
            {
                var keyUser = new UsersPreviewCacheKey(id).ToString();
                if (dictionarUserInCache.TryGetValue(keyUser, out var userPreviewView))
                {
                    listUserPreview.Add(userPreviewView);
                }
                else
                {
                    idUserNotInCache.Add(id);
                }
            }

            if (idUserNotInCache.Count > 0)
            {
                var userNotInCache = await GetPreviewUsersDB(idUserNotInCache);
                listUserPreview.AddRange(userNotInCache);
                await WriteUsersInCache(userNotInCache);
            }

            return listUserPreview;
        }

        public async Task<List<UserPrewievDTO>> GetPreviewUsersDB(List<string>? idUsers)
        {
            if (idUsers == null || !idUsers.Any()) return new List<UserPrewievDTO>();

            return await _userRepository.GetAllUsersQueryable()
                .Where(e => idUsers.Contains(e.Id))
                .ProjectTo<UserPrewievDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<UserPrewievDTO?> GetPreviewUserDB(string idUsers)
        {
            return await _userRepository.GetAllUsersQueryable()
                .ProjectTo<UserPrewievDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(e => e.Id == idUsers);
        }

        public async Task WriteUsersInCache(List<UserPrewievDTO> usersPreview)
        {
            foreach (var userPrev in usersPreview)
            {
                string cacheKey = new UsersPreviewCacheKey(userPrev.Id).ToString();
                await _cache.SetAsync(cacheKey, userPrev,
                    new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24) });
            }
        }

        public async Task<UserPrewievDTO?> GetPreviewUser(string idUser)
        {
            string cacheKey = new UsersPreviewCacheKey(idUser).ToString();
            var (isSuccess, userPreviewView) = await _cache.TryGetValueAsync<UserPrewievDTO>(cacheKey);
            if (isSuccess) return userPreviewView;

            var semaphore = _semaphoresById.GetOrAdd(idUser, _ => new SemaphoreSlim(1, 1));

            await semaphore.WaitAsync();
            try
            {
                (isSuccess, userPreviewView) = await _cache.TryGetValueAsync<UserPrewievDTO>(cacheKey);
                if (isSuccess) return userPreviewView;

                userPreviewView = await GetPreviewUserDB(idUser);

                if (userPreviewView != null)
                {
                    await _cache.SetAsync(cacheKey, userPreviewView,
                        new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24) });
                }
            }
            finally
            {
                semaphore.Release();
            }

            return userPreviewView;
        }

        public record UsersPreviewCacheKey(string userId)
        {
            public override string ToString() => $"users:preview:{userId}";
        }
    }
}
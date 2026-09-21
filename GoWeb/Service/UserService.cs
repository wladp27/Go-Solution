using AutoMapper;
using AutoMapper.QueryableExtensions;
using GoWeb.Interfaces;
using GoWeb.Shared.Models;
using GoWeb.Shared.Сonstants;
using GoWebApplication.Db.Data;
using GoWebApplication.Db.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

namespace GoWeb.Service
{
    public class UserService: IUserService
    {
        private readonly ICacheService cache;
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IUserRepository userRepository;
        public UserService(ICacheService cache, IMapper mapper, IUserRepository userRepository, ApplicationDbContext context) 
        {
            this.cache = cache;
            this.mapper = mapper;
            this.userRepository = userRepository;
            this.context = context;
        }

        public async Task<List<string>> GetIdUsersDB(int idEvent) // без контекста сделать
        {
          
            return await context.UsersEvents.Where(ue => ue.EventId == idEvent &&
                                                 (ue.StatusJoiningId == (int)JoiningStatus.Registered || ue.StatusJoiningId == (int)JoiningStatus.InReserve))
                                            .OrderBy(ue => ue.StatusJoiningId)
                                               .ThenBy(ue => ue.TimeJoinEvent)
                                            .Select(ue=>ue.User.Id)
                                            .ToListAsync();
        }


        public async Task<List<UserPrewievDTO>> GetPreviewUsers(List<string> idUsers)
        {
            var listUserPreview = new List<UserPrewievDTO>();
            var idUserNotInCache = new List<string>();
            var listKey = idUsers.Select(id => new UsersPreviewCacheKey(id).ToString());
            var dictionarUserInCache = await cache.GetManyAsync<UserPrewievDTO>(listKey);
            foreach (var id in idUsers)
            {
                var keyUser = new UsersPreviewCacheKey(id).ToString();
                if (dictionarUserInCache.TryGetValue(keyUser, out UserPrewievDTO? userPreviewView))
                {
                    listUserPreview.Add(userPreviewView);
                }
                else
                {
                    idUserNotInCache.Add(id);
                }          
            }
            if (idUserNotInCache.Count > 0) // ограничение существует
            {
                var userNotInCache = await GetPreviewUsersDB(idUserNotInCache);
                listUserPreview.AddRange(userNotInCache);
                await WriteUsersInCache(userNotInCache);
            }
            return listUserPreview;
        }

        public async Task<List<UserPrewievDTO>> GetPreviewUsersDB(List<string>? idUsers)
        {
            var usersPreview = await userRepository.GetAllUsersQueryable()
                                              .Where(e => idUsers.Contains(e.Id))
                                              .ProjectTo<UserPrewievDTO>(mapper.ConfigurationProvider)
                                              .ToListAsync();
            return usersPreview;
        }


        public async Task<UserPrewievDTO> GetPreviewUserDB(string idUsers)
        {
            var usersPreview = await userRepository.GetAllUsersQueryable()
                                .ProjectTo<UserPrewievDTO>(mapper.ConfigurationProvider)
                                .FirstOrDefaultAsync(e => e.Id == idUsers);
            return usersPreview;
        }



        public async Task WriteUsersInCache(List<UserPrewievDTO> usersPreview)
        {
            foreach (var userPrev in usersPreview)
            {
                await cache.SetAsync(new UsersPreviewCacheKey(userPrev.Id).ToString(), userPrev, 
                               new DistributedCacheEntryOptions(){ AbsoluteExpirationRelativeToNow =  TimeSpan.FromHours(24)});
            }  
        }


        public  async Task<UserPrewievDTO> GetPreviewUser(string idUser)
        {
            var userPreviewView = await cache.GetAsync<UserPrewievDTO>(new UsersPreviewCacheKey(idUser).ToString());
            if (userPreviewView != null)
            {
                return userPreviewView;
            }

            var userPrevDb = await GetPreviewUserDB(idUser);
            if (userPrevDb != null)
            {
                await cache.SetAsync(new UsersPreviewCacheKey(userPrevDb.Id).ToString(), userPrevDb, new DistributedCacheEntryOptions(){ AbsoluteExpirationRelativeToNow =  TimeSpan.FromHours(24)});
            }
            return userPrevDb;
        }


        public record UsersPreviewCacheKey(string userId)
        {
            public override string ToString() => $"users:preview:{userId}";
        }
    }
}

using GoWeb.Shared.Models;

namespace GoWeb.Interfaces
{
    public interface IUserEventService: IUserEvent
    {
        public Task<List<UserPrewievDTO>?> GetUsersEventAsync(int idEvent);
        public Task<EventWithUsersDTO?> GetEventsWithUserAsync(int idEvent);
        public record UsersInEventCacheKey(int idEvent);
    }
}

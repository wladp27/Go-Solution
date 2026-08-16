using GoWeb.Models;
using GoWeb.Shared.Models;
using GoWebApplication.Db.Models;

namespace GoWeb.Interfaces
{
    public interface IEventTypeService
    {
        public Task<EventTypeDTO?> GetByIdAsync(int id);
        public Task<bool> AddAsync(EventTypeDTO eventType);
        public Task<bool> Update(EventTypeDTO eventType);
        public Task<bool> DeleteAsync(EventTypeDTO eventType);
        public Task<List<EventTypeDTO>?> GetAllAsync();
    }
}

using GoWeb.Models;
using GoWeb.Сonstants;
using GoWebApplication.Db.Models;
using System.Security.Claims;
using GoWeb.Shared.Models;
namespace GoWeb.Interfaces
{
    public interface IEventService
    {
      public Task<List<EventSummaryDTO>?> GetFilteredEventsAsync(EventFilterDTO filter);
        public Task<List<CommandViewModel>> GetCommandChekingCanckeledEventAsync();
        public Task<List<CommandViewModel>> GetCommandRecreateEventAsync();
        public Task<int> AddAsync(Event ev);
        public Task<EventFilterDTO> GetDataForFilter();
        public Task<List<EventTypeDTO>> GetTypesEventsForCity(int idCity);
        public Task<EventSummaryDTO?> GetPublichEventByIdAsync(int id);
        public Task<EventSummaryDTO?> GetEventByIdAsync(int id);
        public Task<bool> ExistenceEvent(int idEvent);
        public Task<bool> UpdateStatusEvent(int idEvent, StatusEventConts status);
        public Task<bool> CheckingCountUserAndStatus(int idEvent);
        public Task<bool> DeleteBuIdAsync(int idEvent);
        public Task<EventIndexViewModel> GetFilterEvents(int? selectedCity, int? selectedTypeEvent);
    }
}

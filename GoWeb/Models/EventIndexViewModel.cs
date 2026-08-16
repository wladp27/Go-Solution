using GoWeb.Shared.Models;
namespace GoWeb.Models
{
    public class EventIndexViewModel
    {
        public List<EventSummaryDTO>? result { get; set; }
        public EventFilterDTO Filter { get; set; }
    }
}

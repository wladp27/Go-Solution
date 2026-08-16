
namespace GoWeb.Shared.Models
{
    public class EventWithUsersDTO: EventSummaryDTO
    {
 

        public List<UserPrewievDTO> UsersRegistered { get; set; }
        public List<UserPrewievDTO> UsersInReserve { get; set; }

    }
}

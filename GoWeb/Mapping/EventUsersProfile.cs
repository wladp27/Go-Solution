using AutoMapper;
using GoWeb.Models;
using GoWebApplication.Db.Models;
using GoWeb.Shared.Models;

namespace GoWeb.Mapping
{
    public class EventUsersProfile:Profile
    {
        public EventUsersProfile()
        {
            CreateMap<EventWithUsersDTO, Event>()
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.Organizer, opt => opt.Ignore());

            CreateMap<Event, EventWithUsersDTO>()
             .ForMember(dest => dest.OrganizerName, opt => opt.MapFrom(x => x.Organizer.UserName));

            CreateMap<EventWithUsersDTO, EventSummaryDTO>();
            CreateMap<EventSummaryDTO, EventWithUsersDTO>();
        }
    }
}

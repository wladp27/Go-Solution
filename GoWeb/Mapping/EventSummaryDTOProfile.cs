using AutoMapper;
using GoWeb.Models;
using GoWeb.Сonstants;
using GoWebApplication.Db.Models;
using GoWeb.Shared.Models;

namespace GoWeb.Mapping
{
    public class EventSummaryDTOProfile:Profile
    {
        public EventSummaryDTOProfile()
        {
            CreateMap<EventSummaryDTO, Event>()
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.Organizer, opt => opt.Ignore());

            CreateMap<Event, EventSummaryDTO>()
             .ForMember(dest => dest.OrganizerName, opt => opt.MapFrom(x => x.Organizer.DisplayName))
             .ForMember(dest=>dest.CountRegisteredUsers, opt=> opt.MapFrom(x=>x.UserEvents.Where(u=>u.StatusJoiningId==(int)JoiningStatus.Registered).Count()))
             .ForMember(dest=>dest.ImagePath, opt=> opt.MapFrom(x=> string.IsNullOrEmpty(x.ImagePath) ? "/images/eventsTypes/" + "volleyball.jpg" : "/images/events/" + x.ImagePath));

            CreateMap<EventWithUsersDTO, EventSummaryDTO>();
            CreateMap<EventSummaryDTO, EventWithUsersDTO>();

        }
    }
}

using AutoMapper;
using GoWeb.Models;
using GoWeb.Shared.Models;
using GoWebApplication.Db.Models;

namespace GoWeb.Mapping
{
    public class EventTypeProfile :Profile
    {
        public EventTypeProfile() 
        {
            CreateMap<EventType, EventTypeDTO>()
                .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(x => "images/eventsTypes/" + x.ImagePath)); ;
            CreateMap<EventTypeDTO, EventType>();
        }
    }
}

using AutoMapper;
using GoWeb.Shared.Model;
using GoWebApplication.Db.Models;

namespace GoWeb.Mapping
{
    public class EventDTOProfile : Profile
    {
        public EventDTOProfile()
        {
            CreateMap<Event, EventDTO>();
            CreateMap<EventDTO, Event>();
        }
    }
}

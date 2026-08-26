using AutoMapper;
using GoWeb.Models;
using GoWeb.Shared.Model;
using GoWebApplication.Db.Models;

namespace GoWeb.Mapping
{
    public class LocationPreviewDTOProfile : Profile
    {
        public LocationPreviewDTOProfile()
        {
            CreateMap<Location, LocationPreviewDTO>().ReverseMap();
        }
    }
}
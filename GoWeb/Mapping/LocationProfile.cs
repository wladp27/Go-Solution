using AutoMapper;
using GoWeb.Models;
using GoWebApplication.Db.Models;

namespace GoWeb.Mapping
{
    public class LocationProfile : Profile
    {
        public LocationProfile() 
        {
            CreateMap<Location,LocationCreateViewModel>().ReverseMap();
            CreateMap<LocationCreateViewModel, Location>()
             .ForMember(dest => dest.PhotosLocations, opt => opt.MapFrom(x => x.imagesPaths.Select(img => new PhotosLocation {PhotoPath=img})));
        }
        
    }
}

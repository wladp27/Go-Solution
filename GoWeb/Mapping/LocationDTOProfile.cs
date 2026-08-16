using AutoMapper;
using GoWeb.Shared.Models;
using GoWebApplication.Db.Models;

namespace GoWeb.Mapping
{
    public class LocationDTOProfile : Profile
    {
        public LocationDTOProfile() 
        {
            CreateMap<Location,LocationDTO>()
                             .ForMember(dest => dest.Address, opt => opt.MapFrom(x => $"{x.City.NameCity}, {x.Address}"));
            CreateMap<LocationDTO, Location>()
             .ForMember(dest => dest.PhotosLocations, opt => opt.MapFrom(x => x.imagesPaths.Select(img => new PhotosLocation { PhotoPath = img })));
        }
        
    }
}

using AutoMapper;
using GoWeb.Models;
using GoWebApplication.Db.Models;
using GoWeb.Shared.Models;

namespace GoWeb.Mapping
{
    public class CityProfile : Profile
    {
        public CityProfile() 
        {
            CreateMap<City,CityDTO>().ReverseMap();
            
        }
    }
}

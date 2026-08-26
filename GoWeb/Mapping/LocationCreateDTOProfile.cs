using AutoMapper;
using GoWeb.Shared.Model;
using GoWeb.Shared.Models;
using GoWebApplication.Db.Models;

namespace GoWeb.Mapping
{
    public class LocationCreateDTOProfile:Profile
    {
        public LocationCreateDTOProfile()
        {
            CreateMap<LocationCreateDTO, Location>();
        }
        
    }
}

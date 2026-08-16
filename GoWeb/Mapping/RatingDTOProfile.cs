using AutoMapper;
using GoWeb.Shared.Models;
using GoWebApplication.Db.Models;


namespace GoWeb.Mapping
{
    public class RatingDTOProfile : Profile
    {
        public RatingDTOProfile() 
        {
            CreateMap<Rating, RatingDTO>().ReverseMap();
        }
    }
}

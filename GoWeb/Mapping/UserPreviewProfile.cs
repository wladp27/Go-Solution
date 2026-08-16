using GoWeb.Shared.Models;
using GoWebApplication.Db.Models;
using AutoMapper;

namespace GoWeb.Mapping
{
    public class UserPreviewProfile: Profile
    {
        public UserPreviewProfile()
        {
            CreateMap<User, UserPrewievDTO>().ReverseMap();
            CreateMap<UserEvent, UserPrewievDTO>()
                    .ForMember(dest => dest.UserName, opt => opt.MapFrom(a => a.User.UserName))
                    .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(a => a.User.DisplayName))
                    .ForMember(dest => dest.Ratings, opt => opt.MapFrom(a => a.User.Ratings));
                    //.ForMember(dest=> dest.Ratings,opt=> opt.MapFrom(a=> a.User.Ratings.ToDictionary(k=>k.EventTypeId,v=>v.Value)));
        }
    }
}

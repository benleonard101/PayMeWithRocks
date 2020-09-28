using AutoMapper;

namespace PayMeWithRocks.Application.UserRoles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //CreateMap<IdentityUser, UserDto>()
            //    .ForMember(dest => dest.EmailAddress, opt => opt.MapFrom(src => src.UserName))
            //    .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));
        }
    }
}
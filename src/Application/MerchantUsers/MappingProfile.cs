using AutoMapper;
using Microsoft.AspNetCore.Identity;
using PayMeWithRocks.Application.MerchantUsers.Queries;
using PayMeWithRocks.Domain.Entities;

namespace PayMeWithRocks.Application.MerchantUsers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<MerchantUser, MerchantUserDto>()
                .ForMember(dest => dest.MerchantName, opt => opt.MapFrom(src => src.Merchant.MerchantName))
                .ForMember(x => x.MerchantUserId, opt => opt.Ignore());

            CreateMap<IdentityUser, MerchantUserDto>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.MerchantUserId, opt => opt.MapFrom(src => src.Id));
        }
    }
}
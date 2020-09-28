using AutoMapper;
using PayMeWithRocks.Application.Merchants.Queries;
using PayMeWithRocks.Domain.Entities;

namespace PayMeWithRocks.Application.Merchants
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Merchant, MerchantDto>();
        }
    }
}
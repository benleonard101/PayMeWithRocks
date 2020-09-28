using AutoMapper;
using PayMeWithRocks.Domain.Entities;

namespace PayMeWithRocks.Application.Transactions.Queries
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Transaction, TransactionDto>()
                .ForMember(dest => dest.MerchantName, opt => opt.MapFrom(src => src.Merchant.MerchantName));
        }
    }
}
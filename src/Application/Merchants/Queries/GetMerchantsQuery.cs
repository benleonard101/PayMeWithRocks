using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PayMeWithRocks.Application.Common.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PayMeWithRocks.Application.Merchants.Queries
{
    public class GetMerchantsQuery : IRequest<MerchantsVm>
    {
    }

    public class GetMerchantsQueryHandler : IRequestHandler<GetMerchantsQuery, MerchantsVm>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetMerchantsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<MerchantsVm> Handle(GetMerchantsQuery request, CancellationToken cancellationToken)
        {
            var merchants = await _context.Merchant.ToListAsync(cancellationToken);

            var merchantDtos = _mapper.Map<IList<MerchantDto>>(merchants);

            return new MerchantsVm { Merchants = merchantDtos };
        }
    }
}
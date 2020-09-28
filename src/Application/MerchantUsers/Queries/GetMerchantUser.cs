using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PayMeWithRocks.Application.Authorization;
using PayMeWithRocks.Application.Common.Exceptions;
using PayMeWithRocks.Application.Common.Interfaces;
using PayMeWithRocks.Domain.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PayMeWithRocks.Application.MerchantUsers.Queries
{
    public class GetMerchantUserQuery : IRequest<MerchantUserVm>
    {
        public string MerchantUserId { get; set; }
    }

    public class GetMerchantUserQueryHandler : IRequestHandler<GetMerchantUserQuery, MerchantUserVm>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identity;
        private readonly IMapper _mapper;

        public GetMerchantUserQueryHandler(IApplicationDbContext context, IIdentityService identity, IMapper mapper)
        {
            _context = context;
            _identity = identity;
            _mapper = mapper;
        }

        public async Task<MerchantUserVm> Handle(GetMerchantUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _identity.GetUserAsync(request.MerchantUserId, cancellationToken);

            if (user == null)
            {
                throw new NotFoundException(nameof(MerchantUser), request.MerchantUserId);
            }

            var merchantUser = await _context.MerchantUser.Include(x => x.Merchant).FirstOrDefaultAsync(x => x.MerchantUserId == request.MerchantUserId, cancellationToken);

            var roles = await _identity.GetUserRolesAsync(user);
            var merchantUserDto = _mapper.Map<MerchantUserDto>(user);

            if (roles.Any())
            {
                Enum.TryParse(roles.First(), out UserRole userRole);
                merchantUserDto.Role = userRole;
            }

            merchantUserDto.MerchantName = merchantUser?.Merchant.MerchantName;
            merchantUserDto.MerchantId = merchantUser?.Merchant.MerchantId;

            return new MerchantUserVm
            {
                MerchantUser = merchantUserDto
            };
        }
    }
}
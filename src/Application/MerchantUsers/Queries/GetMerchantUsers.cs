using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PayMeWithRocks.Application.Authorization;
using PayMeWithRocks.Application.Common.Interfaces;
using PayMeWithRocks.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PayMeWithRocks.Application.MerchantUsers.Queries
{
    public class GetMerchantUsersQuery : IRequest<Result<MerchantUsersVm>>
    {
    }

    public class GetMerchantUsersQueryHandler : IRequestHandler<GetMerchantUsersQuery, Result<MerchantUsersVm>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identity;

        public GetMerchantUsersQueryHandler(IApplicationDbContext context, IMapper mapper, IIdentityService identity)
        {
            _context = context;
            _mapper = mapper;
            _identity = identity;
        }

        public async Task<Result<MerchantUsersVm>> Handle(GetMerchantUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _identity.GetAllUsersAsync(cancellationToken);
            var merchantUsers = await _context.MerchantUser
                .Include(x => x.Merchant)
                .ToListAsync(cancellationToken);

            var merchantUserDtos = new List<MerchantUserDto>();

            foreach (var user in users)
            {
                var roles = await _identity.GetUserRolesAsync(user);
                var merchantUserDto = _mapper.Map<MerchantUserDto>(user);
                var role = roles.Any() ? roles.First() : string.Empty;

                if (role != UserRole.PayMeWithRocksAdministrators.ToString())
                {
                    if (Enum.TryParse(role, out UserRole userRole))
                    {
                        merchantUserDto.Role = userRole;
                    }

                    var merchantUser = merchantUsers.FirstOrDefault(x => x.MerchantUserId == user.Id);

                    if (merchantUser != null)
                    {
                        _mapper.Map(merchantUser, merchantUserDto);
                    }

                    merchantUserDtos.Add(merchantUserDto);
                }
            }

            return Result<MerchantUsersVm>.Success(new MerchantUsersVm
            {
                MerchantUsers = merchantUserDtos
            });
        }
    }
}
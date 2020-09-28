using MediatR;
using Microsoft.EntityFrameworkCore;
using PayMeWithRocks.Application.Authorization;
using PayMeWithRocks.Application.Common.Interfaces;
using PayMeWithRocks.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace PayMeWithRocks.Application.MerchantUsers.Command
{
    public class UpdateMerchantUserCommand : IRequest<Unit>
    {
        public string MerchantUserId { get; set; }

        public int? MerchantId { get; set; }

        public UserRole? Role { get; set; }
    }

    public class UpdateMerchantUserCommandHandler : IRequestHandler<UpdateMerchantUserCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identity;

        public UpdateMerchantUserCommandHandler(IApplicationDbContext context, IIdentityService identity)
        {
            _context = context;
            _identity = identity;
        }

        public async Task<Unit> Handle(UpdateMerchantUserCommand request, CancellationToken cancellationToken)
        {
            var entity =
                await _context.MerchantUser
                    .FirstOrDefaultAsync(x => x.MerchantUserId == request.MerchantUserId, cancellationToken);

            if (entity == null && request.MerchantId.HasValue)
            {
                await _context.MerchantUser.AddAsync(new MerchantUser
                {
                    MerchantUserId = request.MerchantUserId,
                    MerchantId = request.MerchantId.Value
                }, cancellationToken);
            }
            else if (entity != null && request.MerchantId.HasValue)
            {
                entity.MerchantId = request.MerchantId.Value;
            }
            else if (entity != null && !request.MerchantId.HasValue)
            {
                _context.MerchantUser.Remove(entity);
            }

            await _identity.RemoveUserFromRolesAsync(request.MerchantUserId);

            if (request.Role.HasValue)
            {
                await _identity.AddUserToRoleAsync(request.MerchantUserId, request.Role.Value);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
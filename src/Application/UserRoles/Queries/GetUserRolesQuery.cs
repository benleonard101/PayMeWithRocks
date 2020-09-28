using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PayMeWithRocks.Application.Common.Interfaces;

namespace PayMeWithRocks.Application.UserRoles.Queries
{
    public class GetUserRolesQuery : IRequest<UserRolesVm>
    {
    }

    public class GetUserRolesQueryHandler : IRequestHandler<GetUserRolesQuery, UserRolesVm>
    {
        private readonly IIdentityService _identity;

        private readonly Dictionary<string, string> _userRoles;

        public GetUserRolesQueryHandler(IIdentityService identity)
        {
            _identity = identity;
            _userRoles = new Dictionary<string, string>();
        }

        public async Task<UserRolesVm> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
        {
            var roles = await _identity.GetAllRolesAsync(cancellationToken);

            return new UserRolesVm { Roles = roles ?? _userRoles };
        }
    }
}
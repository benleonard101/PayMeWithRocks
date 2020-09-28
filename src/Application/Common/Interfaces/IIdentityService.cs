using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PayMeWithRocks.Application.Authorization;

namespace PayMeWithRocks.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<IList<IdentityUser>> GetAllUsersAsync(CancellationToken cancellationToken);

        Task<IdentityUser> GetUserAsync(string userId, CancellationToken cancellationToken);

        Task<IList<string>> GetUserRolesAsync(IdentityUser user);

        Task<Dictionary<string, string>> GetAllRolesAsync(CancellationToken cancellationToken);

        Task AddUserToRoleAsync(string userId, UserRole role);

        Task RemoveUserFromRolesAsync(string userId);
    }
}
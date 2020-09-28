using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PayMeWithRocks.Application.Authorization;
using PayMeWithRocks.Application.Common.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace PayMeWithRocks.Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<IdentityService> _logger;

        public IdentityService(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ILogger<IdentityService> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        public async Task<IList<IdentityUser>> GetAllUsersAsync(CancellationToken cancellationToken)
        {
            return await _userManager.Users.ToListAsync(cancellationToken);
        }

        public async Task<IdentityUser> GetUserAsync(string userId, CancellationToken cancellationToken)
        {
            return await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
        }

        public async Task<IList<string>> GetUserRolesAsync(IdentityUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<Dictionary<string, string>> GetAllRolesAsync(CancellationToken cancellationToken)
        {
            return await _roleManager.Roles.ToDictionaryAsync(x => x.Id, x => x.Name, cancellationToken);
        }

        public async Task AddUserToRoleAsync(string userId, UserRole role)
        {
            _logger.LogInformation($"Adding role {role} to userid {userId}");

            var user = await _userManager.FindByIdAsync(userId);
            if (!await _userManager.IsInRoleAsync(user, role.ToString()))
            {
                _logger.LogInformation("Adding sysadmin to Admin role");
                var userResult = await _userManager.AddToRoleAsync(user, role.ToString());

                if (!userResult.Succeeded)
                {
                    _logger.LogError($"Failed to add role {role} to userid { userId}", userResult.Errors);

                    throw new Exception();
                }
            }
        }

        public async Task RemoveUserFromRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var roles = await _userManager.GetRolesAsync(user);

            _logger.LogInformation($"Removing role(s) {string.Join(",", roles)} for userid {userId}");
            var userResult = await _userManager.RemoveFromRolesAsync(user, roles.ToArray());

            if (!userResult.Succeeded)
            {
                _logger.LogError($"Failed to remove roles {string.Join(",", roles)} for userid { userId}", userResult.Errors);

                throw new Exception();
            }
        }
    }
}
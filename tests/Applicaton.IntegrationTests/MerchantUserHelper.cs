using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using PayMeWithRocks.Application.Authorization;
using System;
using System.Threading.Tasks;

namespace PayMeWithRocks.Application.IntegrationTests
{
    public static class MerchantUserHelper
    {
        public static async Task<string> CreateUser(IServiceProvider serviceProvider, string email, UserRole? role)
        {
            var userId = await EnsureUser(serviceProvider, "Administrator1!", email);

            if (role.HasValue)
            {
                await EnsureRole(serviceProvider, userId, role.ToString());
            }

            return userId;
        }

        private static async Task<string> EnsureUser(IServiceProvider serviceProvider, string testUserPw, string userName)
        {
            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            var user = await userManager.FindByNameAsync(userName);
            if (user == null)
            {
                user = new IdentityUser
                {
                    UserName = userName,
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(user, testUserPw);
            }

            if (user == null)
            {
                throw new Exception("The password is probably not strong enough!");
            }

            return user.Id;
        }

        private static async Task<IdentityResult> EnsureRole(IServiceProvider serviceProvider, string uid, string role)
        {
            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            var user = await userManager.FindByIdAsync(uid);

            if (user == null)
            {
                throw new Exception("The testUserPw password was probably not strong enough!");
            }

            await CreateRole(serviceProvider, role);

            var identityResult = await userManager.AddToRoleAsync(user, role);

            return identityResult;
        }

        private static async Task CreateRole(IServiceProvider serviceProvider, string role)
        {
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            if (roleManager == null)
            {
                throw new Exception("roleManager null");
            }

            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
}
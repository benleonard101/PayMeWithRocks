using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using PayMeWithRocks.Application.Authorization;
using PayMeWithRocks.Domain.Entities;
using PayMeWithRocks.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PayMeWithRocks.Infrastructure.Persistence
{
    public static class ApplicationDbContextSeed
    {
        public static async Task Initialize(IServiceProvider serviceProvider, ApplicationDbContext context)
        {
            var adminId = await EnsureUser(serviceProvider, "Administrator1!", "admin@paymewithrocks.com");
            await EnsureRole(serviceProvider, adminId, UserRole.PayMeWithRocksAdministrators.ToString());

            var merchantAccountId = await EnsureUser(serviceProvider, "Administrator1!", "merchantAccountAdmin@paymewithrocks.com");
            await EnsureRole(serviceProvider, merchantAccountId, UserRole.MerchantAccountAdministrators.ToString());

            var customerServiceId = await EnsureUser(serviceProvider, "Administrator1!", "customerService@paymewithrocks.com");
            await EnsureRole(serviceProvider, customerServiceId, UserRole.CustomerService.ToString());

            await SeedSampleDataAsync(context, merchantAccountId);
        }

        private static async Task<string> EnsureUser(IServiceProvider serviceProvider, string testUserPw, string UserName)
        {
            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            var user = await userManager.FindByNameAsync(UserName);
            if (user == null)
            {
                user = new IdentityUser
                {
                    UserName = UserName,
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

        public static async Task SeedSampleDataAsync(ApplicationDbContext context, string merchantAccountId)
        {
            // Seed, if necessary
            if (!context.Transaction.Any())
            {
                var coldwaterMerchant = new Merchant
                {
                    MerchantName = "Coldwater Creek",
                    MerchantType = MerchantType.StandardMerchant
                };

                var raysMerchant = new Merchant
                {
                    MerchantName = "Rays Carwash",
                    MerchantType = MerchantType.PayFac
                };

                context.Merchant.Add(coldwaterMerchant);
                context.Merchant.Add(raysMerchant);

                await context.SaveChangesAsync();

                var merchantUser = new MerchantUser
                {
                    MerchantId = coldwaterMerchant.MerchantId,
                    MerchantUserId = merchantAccountId
                };

                context.MerchantUser.Add(merchantUser);

                await context.SaveChangesAsync();

                var transactions = new List<Transaction> {
                    new Transaction { MerchantId = coldwaterMerchant.MerchantId, Amount = 100000, TransactionType = TransactionType.Credit },
                    new Transaction { MerchantId = coldwaterMerchant.MerchantId, Amount = 155665.59, TransactionType = TransactionType.Debit },
                    new Transaction { MerchantId = coldwaterMerchant.MerchantId, Amount = 0.59, TransactionType = TransactionType.Debit },
                    new Transaction { MerchantId = raysMerchant.MerchantId, Amount = 4.99, TransactionType = TransactionType.Debit },
                    new Transaction { MerchantId = raysMerchant.MerchantId, Amount = 10.50, TransactionType = TransactionType.Credit },
                    new Transaction { MerchantId = raysMerchant.MerchantId, Amount = 40.00, TransactionType = TransactionType.Debit },
                    new Transaction { MerchantId = raysMerchant.MerchantId, Amount = 5.00, TransactionType = TransactionType.Debit },
                    new Transaction { MerchantId = raysMerchant.MerchantId, Amount = 4.50, TransactionType = TransactionType.Debit }
                };

                context.Transaction.AddRange(transactions);

                await context.SaveChangesAsync();
            }
        }
    }
}
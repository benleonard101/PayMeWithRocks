using FluentAssertions;
using MediatR;
using NUnit.Framework;
using PayMeWithRocks.Application.MerchantUsers.Command;
using PayMeWithRocks.Domain.Entities;
using System;
using System.Threading.Tasks;
using PayMeWithRocks.Application.Authorization;
using PayMeWithRocks.Application.MerchantUsers.Queries;

namespace PayMeWithRocks.Application.IntegrationTests.MerchantUsers.Commands
{
    using static Testing;

    public class UpdateMerchantUserTests : TestBase
    {
        [Test]
        public async Task ShouldUpdateMerchantUserForUserAlreadyAssignedAMerchant()
        {
            await SetupAll();
            var userId = await RunAsDefaultUserAsync();

            var command = new UpdateMerchantUserCommand
            {
                MerchantUserId = userId,
                MerchantId = 1,
                Role = UserRole.CustomerService
            };

            var result = await SendAsync(command);

            result.Should().BeOfType<Unit>();

            var merchantUser = await FindAsync<MerchantUser>(userId);

            merchantUser.Should().NotBeNull();
            merchantUser.MerchantId.Should().Be(1);
            merchantUser.MerchantUserId.Should().Be(userId);
            merchantUser.CreatedBy.Should().Be(userId);
            merchantUser.Created.Should().BeCloseTo(DateTime.Now.ToUniversalTime(), 10000);
        }

        [Test]
        public async Task ShouldUpdateMerchantUserForUserWithNoMerchantAssigned()
        {
            await SetupAll();
            var userId = await RunAsDefaultUserAsync();
            await AddAsync(new MerchantUser { MerchantUserId = userId, MerchantId = 1 });
            var command = new UpdateMerchantUserCommand { MerchantUserId = userId, MerchantId = 2, Role = null };

            var result = await SendAsync(command);

            result.Should().BeOfType<Unit>();

            var merchantUser = await FindAsync<MerchantUser>(userId);

            merchantUser.Should().NotBeNull();
            merchantUser.MerchantId.Should().Be(2);
            merchantUser.MerchantUserId.Should().Be(userId);
            merchantUser.CreatedBy.Should().Be(userId);
            merchantUser.Created.Should().BeCloseTo(DateTime.Now.ToUniversalTime(), 10000);
            merchantUser.LastModifiedBy.Should().Be(userId);
            merchantUser.LastModified.Should().BeCloseTo(DateTime.Now.ToUniversalTime(), 10000);
        }

        [Test]
        public async Task ShouldRemoveLinkToMerchantForAUserWhoPreviouslyHadALink()
        {
            await SetupAll();
            var userId = await RunAsDefaultUserAsync();
            await AddAsync(new MerchantUser { MerchantUserId = userId, MerchantId = 1 });
            var command = new UpdateMerchantUserCommand { MerchantUserId = userId, MerchantId = null, Role = null };

            var result = await SendAsync(command);

            result.Should().BeOfType<Unit>();

            var merchantUser = await FindAsync<MerchantUser>(userId);

            merchantUser.Should().BeNull();
        }

        [Test]
        public async Task ShouldRemoveRoleFromMerchantUser()
        {
            await SetupAll();
            var userId = await CreateMerchantUser("test@gmail.com", UserRole.CustomerService);
            await AddAsync(new MerchantUser { MerchantUserId = userId, MerchantId = 1 });
            var command = new UpdateMerchantUserCommand { MerchantUserId = userId, MerchantId = null, Role = null };

            var result = await SendAsync(command);

            result.Should().BeOfType<Unit>();

            var merchantUserVm = await SendAsync(new GetMerchantUserQuery { MerchantUserId = userId });
            merchantUserVm.MerchantUser.Role.Should().BeNull();
        }
    }
}
using FluentAssertions;
using NUnit.Framework;
using PayMeWithRocks.Application.Authorization;
using PayMeWithRocks.Application.MerchantUsers.Queries;
using PayMeWithRocks.Domain.Entities;
using System.Threading.Tasks;
using PayMeWithRocks.Application.Common.Exceptions;

namespace PayMeWithRocks.Application.IntegrationTests.MerchantUsers.Queries
{
    using static Testing;

    public class GetMerchantUserTests : TestBase
    {
        [Test]
        public async Task ShouldReturnMerchantUser()
        {
            await SetupAll();
            var merchantUserId = await CreateMerchantUser("new@gmail.com", UserRole.CustomerService);
            await AddAsync(new Merchant { MerchantName = "New Merchant" });
            await AddAsync(new MerchantUser { MerchantUserId = merchantUserId, MerchantId = 1 });

            var query = new GetMerchantUserQuery { MerchantUserId = merchantUserId };

            var result = await SendAsync(query);

            result.MerchantUser.MerchantId.Should().Be(1);
            result.MerchantUser.MerchantUserId.Should().Be(merchantUserId);
            result.MerchantUser.MerchantName.Should().Be("Coldwater Creek");
            result.MerchantUser.Role.Should().Be(UserRole.CustomerService);
        }

        [Test]
        public void ShouldThrowNotFoundExceptionWithInvalidMerchantUser()
        {
            var query = new GetMerchantUserQuery { MerchantUserId = "99" };

            FluentActions.Invoking(() =>
                SendAsync(query)).Should().Throw<NotFoundException>();
        }
    }
}
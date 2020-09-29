using FluentAssertions;
using NUnit.Framework;
using PayMeWithRocks.Application.Merchants.Queries;
using PayMeWithRocks.Domain.Enums;
using System.Linq;
using System.Threading.Tasks;

namespace PayMeWithRocks.Application.IntegrationTests.Merchants.Queries
{
    using static Testing;

    public class GetMerchantsTests : TestBase
    {
        [Test]
        public async Task ShouldReturnMerchants()
        {
            await SetupAll();

            var result = await SendAsync(new GetMerchantsQuery());

            result.Merchants.Should().HaveCount(2);

            var coldwaterMerchant = result.Merchants.First(x => x.MerchantName == "Coldwater Creek");
            coldwaterMerchant.MerchantId.Should().Be(1);
            coldwaterMerchant.MerchantType.Should().Be(MerchantType.StandardMerchant);

            var raysCarwash = result.Merchants.First(x => x.MerchantName == "Rays Carwash");
            raysCarwash.MerchantId.Should().Be(1);
            raysCarwash.MerchantType.Should().Be(MerchantType.PayFac);
        }
    }
}
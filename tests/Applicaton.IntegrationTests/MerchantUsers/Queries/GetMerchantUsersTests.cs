using FluentAssertions;
using NUnit.Framework;
using PayMeWithRocks.Application.Authorization;
using PayMeWithRocks.Application.MerchantUsers.Queries;
using System.Linq;
using System.Threading.Tasks;

namespace PayMeWithRocks.Application.IntegrationTests.MerchantUsers.Queries
{
    using static Testing;

    public class GetMerchantUsersTests : TestBase
    {
        [Test]
        public async Task ShouldReturnMerchantUsers()
        {
            await SetupAll();

            var result = await SendAsync(new GetMerchantUsersQuery());

            result.Value.MerchantUsers.Should().NotContain(x => x.Role == UserRole.PayMeWithRocksAdministrators);
            result.Value.MerchantUsers.Should().HaveCount(2);

            var merchantAccountAdmin = result.Value.MerchantUsers.First(x => x.Email == "merchantAccountAdmin@paymewithrocks.com");
            merchantAccountAdmin.MerchantUserId.Should().NotBeNull();
            merchantAccountAdmin.MerchantId.Should().NotBeNull();
            merchantAccountAdmin.MerchantName.Should().Be("Coldwater Creek");
            merchantAccountAdmin.Role.Value.Should().Be(UserRole.MerchantAccountAdministrators);

            var customerServiceUser = result.Value.MerchantUsers.First(x => x.Email == "customerService@paymewithrocks.com");
            customerServiceUser.MerchantUserId.Should().NotBeNull();
            customerServiceUser.MerchantId.Should().BeNull();
            customerServiceUser.MerchantName.Should().BeNull();
            customerServiceUser.Role.Value.Should().Be(UserRole.CustomerService);
        }

        //[Test]
        //public async Task ShouldReturnAllListsAndItems()
        //{
        //    await AddAsync(new TodoList
        //    {
        //        Title = "Shopping",
        //        Items =
        //            {
        //                new TodoItem { Title = "Apples", Done = true },
        //                new TodoItem { Title = "Milk", Done = true },
        //                new TodoItem { Title = "Bread", Done = true },
        //                new TodoItem { Title = "Toilet paper" },
        //                new TodoItem { Title = "Pasta" },
        //                new TodoItem { Title = "Tissues" },
        //                new TodoItem { Title = "Tuna" }
        //            }
        //    });

        //    var query = new GetTodosQuery();

        //    var result = await SendAsync(query);

        //    result.Lists.Should().HaveCount(1);
        //    result.Lists.First().Items.Should().HaveCount(7);
        //}
    }
}
using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using PayMeWithRocks.Application.Transactions.Queries;
using PayMeWithRocks.Domain.Enums;
using System.Threading.Tasks;

namespace PayMeWithRocks.Application.IntegrationTests.Transactions.Queries
{
    using static Testing;

    public class GetTransactionsQueryTests : TestBase
    {
        [Test]
        public async Task ShouldReturnTransactions()
        {
            await SetupAll();

            var result = await SendAsync(new GetTransactionsQuery { MerchantId = 1 });

            result.Transactions.Should().HaveCount(3);

            var expectedTransactions = new List<TransactionDto>
            {
                new TransactionDto
                {
                    MerchantName = "Coldwater Creek", Amount = 100000, TransactionType = TransactionType.Credit
                },

                new TransactionDto
                {
                    MerchantName = "Coldwater Creek", Amount = 155665.59, TransactionType = TransactionType.Debit
                },
                new TransactionDto
                {
                    MerchantName = "Coldwater Creek", Amount = 0.59, TransactionType = TransactionType.Debit
                }
            };

            result.Transactions.Should().BeEquivalentTo(expectedTransactions, option =>
                 option.Using<DateTime>(x => x.Subject.Should().BeCloseTo(DateTime.Now.ToUniversalTime(), 1000))
                     .WhenTypeIs<DateTime>());
        }
    }
}
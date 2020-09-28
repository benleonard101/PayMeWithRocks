using PayMeWithRocks.Domain.Enums;
using System;

namespace PayMeWithRocks.Application.Transactions.Queries
{
    public class TransactionDto
    {
        public string MerchantName { get; set; }

        public double Amount { get; set; }

        public TransactionType TransactionType { get; set; }

        public DateTime Created { get; set; }
    }
}
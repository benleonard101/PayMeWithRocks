using System.Collections.Generic;

namespace PayMeWithRocks.Application.Transactions.Queries
{
    public class TransactionsVm
    {
        public IList<TransactionDto> Transactions { get; set; }
    }
}
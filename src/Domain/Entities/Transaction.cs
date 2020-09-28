using PayMeWithRocks.Domain.Common;
using PayMeWithRocks.Domain.Enums;

namespace PayMeWithRocks.Domain.Entities
{
    public class Transaction : AuditableEntity
    {
        public int TransactionId { get; set; }

        public double Amount { get; set; }

        public TransactionType TransactionType { get; set; }

        public int MerchantId { get; set; }

        public Merchant Merchant { get; set; }
    }
}
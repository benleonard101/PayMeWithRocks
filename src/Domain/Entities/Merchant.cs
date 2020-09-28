using PayMeWithRocks.Domain.Common;
using PayMeWithRocks.Domain.Enums;
using System.Collections.Generic;

namespace PayMeWithRocks.Domain.Entities
{
    public class Merchant : AuditableEntity
    {
        public int MerchantId { get; set; }

        public string MerchantName { get; set; }

        public MerchantType MerchantType { get; set; }

        public List<Transaction> Transactions { get; set; }
    }
}
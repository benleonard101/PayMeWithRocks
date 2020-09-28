using PayMeWithRocks.Domain.Common;

namespace PayMeWithRocks.Domain.Entities
{
    public class MerchantUser : AuditableEntity
    {
        public string MerchantUserId { get; set; }

        public int MerchantId { get; set; }

        public Merchant Merchant { get; set; }
    }
}
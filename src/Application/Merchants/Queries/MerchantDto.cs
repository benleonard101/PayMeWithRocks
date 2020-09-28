using PayMeWithRocks.Domain.Enums;

namespace PayMeWithRocks.Application.Merchants.Queries
{
    public class MerchantDto
    {
        public int MerchantId { get; set; }

        public string MerchantName { get; set; }

        public MerchantType MerchantType { get; set; }
    }
}
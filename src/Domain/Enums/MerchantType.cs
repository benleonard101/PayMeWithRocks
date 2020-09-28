using System.ComponentModel;

namespace PayMeWithRocks.Domain.Enums
{
    public enum MerchantType
    {
        [Description("Merchant")]
        StandardMerchant = 0,

        [Description("PayFac Aggregate")]
        PayFac,

        [Description("Shared Merchant Location")]
        SharedMerchantLocation
    }
}
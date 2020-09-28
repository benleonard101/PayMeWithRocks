using System.ComponentModel;

namespace PayMeWithRocks.Application.Authorization
{
    public enum UserRole
    {
        [Description("System Admin")]
        PayMeWithRocksAdministrators,

        [Description("Merchant Account Admin")]
        MerchantAccountAdministrators,

        [Description("Customer Service")]
        CustomerService
    }
}
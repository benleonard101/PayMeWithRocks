using PayMeWithRocks.Application.Authorization;

namespace PayMeWithRocks.Application.MerchantUsers.Queries
{
    public class MerchantUserDto
    {
        public string MerchantUserId { get; set; }

        public string Email { get; set; }

        public UserRole? Role { get; set; }

        public int? MerchantId { get; set; }

        public string MerchantName { get; set; }
    }
}
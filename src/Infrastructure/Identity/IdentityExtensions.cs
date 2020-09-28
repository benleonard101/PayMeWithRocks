using System.Security.Claims;

namespace PayMeWithRocks.Infrastructure.Identity
{
    public static class IdentityExtensions
    {
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            return principal.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public static int GetMerchantId(this ClaimsPrincipal claimsPrincipal)
        {
            var claim = claimsPrincipal.FindFirst(CustomIdentityConstants.MerchantIdClaim);

            return (claim != null && !string.IsNullOrEmpty(claim.Value)) ? int.Parse(claim.Value) : 0;
        }
    }
}
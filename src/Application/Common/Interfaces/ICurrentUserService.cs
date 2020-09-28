using System.Threading.Tasks;

namespace PayMeWithRocks.Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        string UserId { get; }

        Task<int> GetMerchantId();
    }
}
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using PayMeWithRocks.Application.Common.Interfaces;
using PayMeWithRocks.Application.MerchantUsers.Queries;
using PayMeWithRocks.Infrastructure.Identity;
using PayMeWithRocksUI.Extensions;

namespace PayMeWithRocksUI.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMediator _mediator;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor, IMediator mediator)
        {
            _httpContextAccessor = httpContextAccessor;
            _mediator = mediator;
        }

        public string UserId => _httpContextAccessor.HttpContext?.User?.GetUserId();

        public async Task<int> GetMerchantId()
        {
            var merchantUserResult = await _mediator.Send(new GetMerchantUserQuery { MerchantUserId = UserId });

            return merchantUserResult?.MerchantUser?.MerchantId ?? 0;
        }
    }
}
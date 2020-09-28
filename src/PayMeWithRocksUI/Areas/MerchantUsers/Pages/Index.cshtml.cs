using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PayMeWithRocks.Application.MerchantUsers.Queries;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace PayMeWithRocksUI.Areas.MerchantUsers.Pages
{
    [Authorize(Roles = "PayMeWithRocksAdministrators")]
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;

        public IndexModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public IList<MerchantUserVm> MerchantUsers { get; set; }

        public async Task OnGetAsync()
        {
            var merchantUsersResult = await _mediator.Send(new GetMerchantUsersQuery());
            MerchantUsers = new List<MerchantUserVm>();

            foreach (var user in merchantUsersResult.Value.MerchantUsers)
            {
                var userVm = new MerchantUserVm
                {
                    UserEmail = user.Email,
                    UserId = user.MerchantUserId,
                    UserRole = user.Role?.GetDescription(),
                    MerchantId = user.MerchantId?.ToString() ?? string.Empty,
                    AssignedMerchant = user?.MerchantName ?? string.Empty,
                };

                MerchantUsers.Add(userVm);
            }
        }

        public class MerchantUserVm
        {
            [DisplayName("Email")]
            public string UserEmail { get; set; }

            [DisplayName("Role")]
            public string UserRole { get; set; }

            [DisplayName("Merchant")]
            public string AssignedMerchant { get; set; }

            public string MerchantId { get; set; }

            public string UserId { get; set; }
        }
    }
}
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PayMeWithRocks.Application.Authorization;
using PayMeWithRocks.Application.Merchants.Queries;
using PayMeWithRocks.Application.MerchantUsers.Command;
using PayMeWithRocks.Application.MerchantUsers.Queries;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace PayMeWithRocksUI.Areas.MerchantUsers.Pages
{
    [BindProperties(SupportsGet = true)]
    [Authorize(Roles = "PayMeWithRocksAdministrators")]
    public class EditModel : PageModel
    {
        private readonly IMediator _mediator;

        public EditModel(IMediator mediator)
        {
            _mediator = mediator;
            MerchantUser = new EditMerchantUserVm();
        }

        public EditMerchantUserVm MerchantUser { get; set; }

        [DisplayName("Merchant")]
        public IList<SelectListItem> Merchants { get; set; }

        [DisplayName("Role")]
        public IList<SelectListItem> Roles { get; set; }

        public async Task OnGetAsync(string userId)
        {
            var merchantUserVm = await _mediator.Send(new GetMerchantUserQuery { MerchantUserId = userId });
            MerchantUser.UserEmail = merchantUserVm.MerchantUser.Email;
            MerchantUser.UserId = merchantUserVm.MerchantUser.MerchantUserId;
            MerchantUser.MerchantId = merchantUserVm.MerchantUser?.MerchantId;
            MerchantUser.Role = merchantUserVm.MerchantUser.Role?.ToString() ?? string.Empty;

            //var userRolesVm = await _mediator.Send(new GetUserRolesQuery());
            Roles = Enum.GetValues(typeof(UserRole)).Cast<UserRole>()
                .Where(x => x.ToString() != UserRole.PayMeWithRocksAdministrators.ToString())
                .Select(v => new SelectListItem
                {
                    Text = v.GetDescription(),
                    Value = v.ToString(),
                    Selected = v.ToString() == MerchantUser.Role
                }).ToList();

            var merchantsVm = await _mediator.Send(new GetMerchantsQuery());
            Merchants = merchantsVm.Merchants.Select(g => new SelectListItem
            {
                Value = g.MerchantId.ToString(),
                Text = g.MerchantName,
                Selected = g.MerchantId == merchantUserVm.MerchantUser.MerchantId
            }).ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var role = string.IsNullOrEmpty(MerchantUser.Role) ?
                (UserRole?)null : Enum.Parse<UserRole>(MerchantUser.Role);

            await _mediator.Send(new UpdateMerchantUserCommand
            {
                MerchantId = MerchantUser.MerchantId,
                MerchantUserId = MerchantUser.UserId,
                Role = role
            });

            // If we got this far, something failed, redisplay form
            return new RedirectToPageResult("Index");
        }
    }

    public class EditMerchantUserVm
    {
        [DisplayName("Email Address")]
        public string UserEmail { get; set; }

        public string Role { get; set; }

        public int? MerchantId { get; set; }

        public string UserId { get; set; }
    }
}
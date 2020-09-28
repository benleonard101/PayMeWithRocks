using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PayMeWithRocks.Application.Common.Interfaces;
using PayMeWithRocks.Application.Transactions.Queries;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace PayMeWithRocksUI.Areas.Transactions.Pages
{
    [Authorize(Roles = "MerchantAccountAdministrators")]
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUserService;

        public IndexModel(IMediator mediator, ICurrentUserService currentUserService)
        {
            _mediator = mediator;
            _currentUserService = currentUserService;
        }

        public IList<TransactionDto> Transaction { get; set; }

        public async Task OnGetAsync()
        {
            var transactionsVm = await _mediator.Send(new GetTransactionsQuery { MerchantId = await _currentUserService.GetMerchantId() });

            Transaction = transactionsVm.Transactions;
        }
    }
}
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PayMeWithRocks.Application.Common.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PayMeWithRocks.Application.Transactions.Queries
{
    public class GetTransactionsQuery : IRequest<TransactionsVm>
    {
        public int MerchantId { get; set; }
    }

    public class GetTransactionsQueryHandler : IRequestHandler<GetTransactionsQuery, TransactionsVm>
    {
        private readonly IApplicationDbContext _context;

        private readonly IMapper _mapper;

        public GetTransactionsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TransactionsVm> Handle(GetTransactionsQuery request, CancellationToken cancellationToken)
        {
            var transactions = await _context.Transaction.Include(x => x.Merchant)
                .Where(x => x.MerchantId == request.MerchantId).ToListAsync(cancellationToken);

            var transactionDtoList = _mapper.Map<IList<TransactionDto>>(transactions);

            return new TransactionsVm
            {
                Transactions = transactionDtoList
            };
        }
    }
}
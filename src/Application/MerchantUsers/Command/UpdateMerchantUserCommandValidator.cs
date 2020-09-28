using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PayMeWithRocks.Application.Common.Interfaces;

namespace PayMeWithRocks.Application.MerchantUsers.Command
{
    public class UpdateMerchantUserCommandValidator : AbstractValidator<UpdateMerchantUserCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateMerchantUserCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.Role).IsInEnum();

            RuleFor(v => v.MerchantId)
                .MustAsync(BeAValidMerchantOrNull).WithMessage("The specified merchant id is not valid");
        }

        public async Task<bool> BeAValidMerchantOrNull(int? id, CancellationToken cancellationToken)
        {
            if (!id.HasValue)
            {
                return true;
            }
            else
            {
                return await _context.Merchant
                    .AnyAsync(l => l.MerchantId == id, cancellationToken);
            }
        }
    }
}
using Microsoft.EntityFrameworkCore;
using PayMeWithRocks.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace PayMeWithRocks.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Merchant> Merchant { get; set; }

        DbSet<MerchantUser> MerchantUser { get; set; }

        DbSet<Transaction> Transaction { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
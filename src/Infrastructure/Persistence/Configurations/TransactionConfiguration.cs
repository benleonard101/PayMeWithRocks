using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PayMeWithRocks.Domain.Entities;

namespace PayMeWithRocks.Infrastructure.Persistence.Configurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.Property(e => e.TransactionId)
                 .ValueGeneratedOnAdd();

            builder.Property(t => t.Amount)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(t => t.MerchantId)
                .IsRequired();

            builder.Property(t => t.TransactionType)
                 .IsRequired();
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PayMeWithRocks.Domain.Entities;

namespace PayMeWithRocks.Infrastructure.Persistence.Configurations
{
    public class MerchantConfiguration : IEntityTypeConfiguration<Merchant>
    {
        public void Configure(EntityTypeBuilder<Merchant> builder)
        {
            builder.Property(e => e.MerchantId)
                 .ValueGeneratedOnAdd();

            builder.Property(t => t.MerchantName)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(t => t.MerchantType)
                .IsRequired();
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PayMeWithRocks.Domain.Entities;

namespace PayMeWithRocks.Infrastructure.Persistence.Configurations
{
    public class MerchantUserConfiguration : IEntityTypeConfiguration<MerchantUser>
    {
        public void Configure(EntityTypeBuilder<MerchantUser> builder)
        {
            builder.Property(e => e.MerchantUserId)
                 .ValueGeneratedOnAdd();

            builder.Property(t => t.MerchantId)
                .IsRequired();
        }
    }
}
using CloudBlue.Domain.DataModels.Operations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CloudBlue.Data.Configurations.Crm;

internal class SalesPromotionConfiguration : IEntityTypeConfiguration<SalesPromotion>
{
    public void Configure(EntityTypeBuilder<SalesPromotion> entity)
    {
    entity.HasKey(e => e.Id)
        .HasName("PK__SalesPro__7EE01F91EF461031");

    entity.Property(e => e.Id)
        .UseIdentityAlwaysColumn();

    entity.Property(e => e.Created)
        .HasPrecision(6);

    entity.Property(e => e.LastUpdated)
        .HasPrecision(6);

    entity.Property(e => e.PromotionEndDate)
        .HasPrecision(6);

    entity.Property(e => e.PromotionStartDate)
        .HasPrecision(6);
    }
}
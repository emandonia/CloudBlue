using CloudBlue.Domain.DataModels.Lookups;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CloudBlue.Data.Configurations.Lookups;

internal class VwLookupLocationConfiguration : IEntityTypeConfiguration<VwLookupLocation>
{
    public void Configure(EntityTypeBuilder<VwLookupLocation> entity)
    {
        entity.HasKey(z => z.Id);
        entity

            .ToView("VwLookupLocations");

        entity.Property(e => e.City).HasMaxLength(150);
        entity.Property(e => e.Country).HasMaxLength(50);
        entity.Property(e => e.District).HasMaxLength(250);
        entity.Property(e => e.Neighborhood).HasMaxLength(250);

    }
}
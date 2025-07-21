using CloudBlue.Domain.DataModels.Lookups;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CloudBlue.Data.Configurations.Lookups;

internal class LookUpCityConfiguration : IEntityTypeConfiguration<LookUpCity>
{
    public void Configure(EntityTypeBuilder<LookUpCity> entity)
    {
        entity.HasKey(e => e.Id);

        entity.HasIndex(e => e.CountryId, "countryIdx");


        entity.Property(e => e.City)
            .HasMaxLength(150);

        entity.HasOne(d => d.Country)
            .WithMany(p => p.LookUpCities)
            .HasForeignKey(d => d.CountryId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK__LookUpCit__Count__0F624AF8");
    }
}
using CloudBlue.Domain.DataModels.Lookups;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CloudBlue.Data.Configurations.Lookups;

internal class LookUpDistrictConfiguration : IEntityTypeConfiguration<LookUpDistrict>
{
    public void Configure(EntityTypeBuilder<LookUpDistrict> entity)
    {
        entity.HasKey(e => e.Id);

        entity.HasIndex(e => e.CityId, "CityIdx");



        entity.Property(e => e.District)
            .HasMaxLength(250);

        entity.Property(e => e.DistrictArabic)
            .HasMaxLength(250);

        entity.HasOne(d => d.City)
            .WithMany(p => p.LookUpDistricts)
            .HasForeignKey(d => d.CityId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK__LookUpDis__CityI__114A936A");
    }
}
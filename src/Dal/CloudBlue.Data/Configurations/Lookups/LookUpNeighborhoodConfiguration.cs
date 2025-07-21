using CloudBlue.Domain.DataModels.Lookups;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CloudBlue.Data.Configurations.Lookups;

internal class LookUpNeighborhoodConfiguration : IEntityTypeConfiguration<LookUpNeighborhood>
{
    public void Configure(EntityTypeBuilder<LookUpNeighborhood> entity)
    {
        entity.HasKey(e => e.Id);



        entity.Property(e => e.Latitude)
            .HasPrecision(16, 10);

        entity.Property(e => e.Longitude)
            .HasPrecision(16, 10);

        entity.Property(e => e.Neighborhood)
            .HasMaxLength(250);

        entity.Property(e => e.NeighborhoodNameAra)
            .HasMaxLength(250);

        entity.HasOne(d => d.District)
            .WithMany(p => p.LookUpNeighborhoods)
            .HasForeignKey(d => d.DistrictId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK__LookUpNei__Distr__1332DBDC");
    }
}
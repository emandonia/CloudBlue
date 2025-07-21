using CloudBlue.Domain.DataModels.Lookups;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CloudBlue.Data.Configurations.Lookups;

internal class LookUpKnowSubSourceConfiguration : IEntityTypeConfiguration<LookUpKnowSubSource>
{
    public void Configure(EntityTypeBuilder<LookUpKnowSubSource> entity)
    {
        entity.HasKey(e => e.Id)
             ;

        entity.HasIndex(e => e.KnowSourceId, "KnowSourceIdx");


        entity.Property(e => e.Abbrev)
            .HasMaxLength(255);

        entity.Property(e => e.KnowSubSource)
            .HasMaxLength(255);

        entity.HasOne(d => d.KnowSource)
            .WithMany(p => p.KnowSubSources)
            .HasForeignKey(d => d.KnowSourceId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK__LookUpKno__KnowS__1BC821DD");
    }
}
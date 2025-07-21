using CloudBlue.Domain.DataModels.Lookups;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CloudBlue.Data.Configurations.Users;

internal class ConstructionDeveloperConfiguration : IEntityTypeConfiguration<ConstructionDeveloper>
{
    public void Configure(EntityTypeBuilder<ConstructionDeveloper> entity)
    {
    entity.HasKey(e => e.Id)
        .HasName("PK__Develope__DE0994334D8C551C");

    entity.Property(e => e.Id)
        .UseIdentityAlwaysColumn();

    entity.Property(e => e.DeveloperName)
        .HasMaxLength(500);

    entity.HasMany(z => z.ConstructionDeveloperProjects)
        .WithOne(z => z.Developer)
        .HasForeignKey(z => z.DeveloperId)
        .HasPrincipalKey(z => z.Id);
    }
}
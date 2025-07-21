using CloudBlue.Domain.DataModels.CbUsers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CloudBlue.Data.Configurations.Users;

internal class VwAgentConfiguration : IEntityTypeConfiguration<VwAgent>
{
    public void Configure(EntityTypeBuilder<VwAgent> entity)
    {
        entity.HasKey(z => z.AgentId);
        entity.ToView("VwAgents");

        entity.Property(e => e.AgentName)
            .HasMaxLength(250);

        entity.Property(e => e.Email)
            .HasMaxLength(120);
    }
}
using CloudBlue.Domain.DataModels.CbUsers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CloudBlue.Data.Configurations.Users;

internal class VwCallRecipientConfiguration : IEntityTypeConfiguration<VwCallRecipient>
{
    public void Configure(EntityTypeBuilder<VwCallRecipient> entity)
    {
        entity.HasKey(z => z.UserId);
        entity.ToView("VwCallRecipients");

        entity.Property(e => e.UserFullName)
            .HasMaxLength(250);
    }
}
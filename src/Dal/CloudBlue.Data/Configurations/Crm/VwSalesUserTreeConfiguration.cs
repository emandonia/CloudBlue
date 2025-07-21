using CloudBlue.Domain.DataModels.CbUsers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CloudBlue.Data.Configurations.Crm;

internal class VwSalesUserTreeConfiguration : IEntityTypeConfiguration<VwSalesUserTree>
{
    public void Configure(EntityTypeBuilder<VwSalesUserTree> entity)
    {
        entity.HasKey(z => z.Id);
        entity.ToView("VwSalesUserTrees");

        entity.Property(e => e.FullName)
            .HasMaxLength(250);

        entity.Property(e => e.HireDate)
            .HasPrecision(6);

        entity.Property(e => e.ResignDate)
            .HasPrecision(6);

        entity.Property(e => e.UserPositionName)
            .HasMaxLength(255);
    }
}
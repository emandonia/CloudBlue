using CloudBlue.Domain.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CloudBlue.Data.Configurations.App;

internal class DashboardNotificationConfiguration : IEntityTypeConfiguration<DashboardNotification>
{
    public void Configure(EntityTypeBuilder<DashboardNotification> entity)
    {
        entity.HasKey(e => e.Id).HasName("DashboardNotifications_pkey");
        entity.ToTable("DashboardNotifications");


        entity.Property(e => e.Id).UseIdentityAlwaysColumn();
        entity.Property(e => e.DepartmentId);
        entity.Property(e => e.Label).HasMaxLength(255);
        entity.Property(e => e.PositionId);
        entity.Property(e => e.PropertyName).HasMaxLength(255);
        entity.Property(e => e.SalesAgents);
        entity.Property(e => e.SalesMangers);
    }
}
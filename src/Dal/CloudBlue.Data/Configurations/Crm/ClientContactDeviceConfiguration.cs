using CloudBlue.Domain.DataModels.Crm;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CloudBlue.Data.Configurations.Crm;

internal class ClientContactDeviceConfiguration : IEntityTypeConfiguration<ClientContactDevice>
{
    public void Configure(EntityTypeBuilder<ClientContactDevice> entity)
    {
        entity.HasIndex(e => e.CountryId, "ClientContactDevices_CountryId_idx");
        entity.HasIndex(e => e.DeviceInfo, "ClientContactDevices_DeviceInfo_idx");
        entity.HasIndex(e => e.ClientId, "DeviceClientIdIdx");

        entity.Property(e => e.Id)
            .UseIdentityAlwaysColumn();

        entity.Property(e => e.CountryId)
            ;

        entity.Property(e => e.CreationDate)
            .HasPrecision(6);

        entity.Property(e => e.CreatedById)
            ;

        entity.Property(e => e.DeviceInfo)
            .HasMaxLength(50);

        entity.Property(e => e.DeviceType)
            .HasColumnName("DeviceTypeId")
            .HasConversion<int>();

        entity.Property(e => e.IsDefault)
            ;

        entity.Property(e => e.Phone)
            .HasMaxLength(12);

        entity.Property(e => e.PhoneAreaCode)
            .HasMaxLength(6);

        entity.Property(e => e.PhoneCountryCode)
            .HasMaxLength(8);
    }
}
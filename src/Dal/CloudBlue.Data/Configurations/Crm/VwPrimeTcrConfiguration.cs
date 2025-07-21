using CloudBlue.Domain.DataModels.PrimeTcrs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CloudBlue.Data.Configurations.Crm;

internal class VwPrimeTcrConfiguration : IEntityTypeConfiguration<VwPrimeTcr>
{
    public void Configure(EntityTypeBuilder<VwPrimeTcr> entity)
    {
        entity.HasKey(z => z.Id);
        entity.ToView("VwPrimeTcrs");

        entity.Property(e => e.Agency)
            .HasMaxLength(255);

        entity.Property(e => e.LeadTicketBranchName)
            .HasMaxLength(120);

        entity.Property(e => e.BuildUpArea)
            .HasPrecision(12, 2);

        entity.Property(e => e.ClientName)
            .HasMaxLength(150);

        entity.Property(e => e.ClientNameLowered)
            .HasMaxLength(150);

        entity.Property(e => e.ClosingChannel)
            .HasMaxLength(50);

        entity.Property(e => e.ClosingSubChannel)
            .HasMaxLength(255);

        entity.Property(e => e.CollectedCommissionValue)
            .HasPrecision(12, 2);

        entity.Property(e => e.CompanyCommissionPercentage)
            .HasPrecision(6, 5);

        entity.Property(e => e.CompanyCommissionValue)
            .HasPrecision(12, 2);

        entity.Property(e => e.LeadTicketCompanyName)
            .HasMaxLength(50);

        entity.Property(e => e.RecentEventsJsonb)
            
            .HasColumnType("jsonb");

        entity.Property(e => e.ConfirmedContractingDate)
            .HasPrecision(6);

        entity.Property(e => e.ConfirmedHalfContractingDate)
            .HasPrecision(6);

        entity.Property(e => e.ConfirmedReservingDate)
            .HasPrecision(6);

        entity.Property(e => e.ContactDevicesJsonb)
            .HasColumnType("jsonb");

        entity.Property(e => e.ContractExpectedDate)
            .HasPrecision(6);

        entity.Property(e => e.CreatedBy)
            .HasMaxLength(100);

        entity.Property(e => e.CreationDateTime)
            .HasPrecision(6);

        entity.Property(e => e.DeveloperName)
            .HasMaxLength(100);

        entity.Property(e => e.DocumentDate)
            .HasPrecision(6);

        entity.Property(e => e.DueBalance)
            .HasPrecision(12, 2);

        entity.Property(e => e.ExtraManagerName)
            .HasMaxLength(100);

        entity.Property(e => e.FirstAgentForcedCommissioningPercentage)
            .HasPrecision(6, 5);

        entity.Property(e => e.FirstAgentName)
            .HasMaxLength(100);

        entity.Property(e => e.FirstAgentSharePercentage)
            .HasPrecision(6, 5);

        entity.Property(e => e.FirstAgentTreeJsonb)
            .HasColumnType("jsonb");

        entity.Property(e => e.FirstAgentTreeNames)
            .HasMaxLength(1000);

        entity.Property(e => e.FirstReferralAgentName)
            .HasMaxLength(100);

        entity.Property(e => e.FirstReferralAgentTreeJsonb)
            .HasColumnType("jsonb");

        entity.Property(e => e.FirstReferralAgentTreeNames)
            .HasMaxLength(1000);

        entity.Property(e => e.ForceAchievementPercentage)
            .HasPrecision(6, 5);

        entity.Property(e => e.ForceCommissionPercentage)
            .HasPrecision(6, 5);

        entity.Property(e => e.ForcedAgentIncentiveValue)
            .HasPrecision(12, 2);

        entity.Property(e => e.ForcedScaledCommissionPercentage)
            .HasPrecision(6, 5);

        entity.Property(e => e.FreezeCommissionAgentsIds)
            .HasMaxLength(255);

        entity.Property(e => e.FreezeCommissionManagersIds)
            .HasMaxLength(255);

        entity.Property(e => e.LandArea)
            .HasPrecision(12, 2);

        entity.Property(e => e.LastConflictDate)
            .HasPrecision(6);

        entity.Property(e => e.LastDeveloperFeedBack)
            .HasMaxLength(2500);

        entity.Property(e => e.LastDeveloperFeedBackDate)
            .HasPrecision(6);

        entity.Property(e => e.LastDeveloperReviewingDate)
            .HasPrecision(6);

        entity.Property(e => e.LastFeedBack)
            .HasMaxLength(5000);

        entity.Property(e => e.LastMarketingChannel)
            .HasMaxLength(50);

        entity.Property(e => e.LastMarketingSubChannel)
            .HasMaxLength(255);

        entity.Property(e => e.LastPostponeDate)
            .HasPrecision(6);

        entity.Property(e => e.LastReopenDate)
            .HasPrecision(6);

        entity.Property(e => e.LastResolveDate)
            .HasPrecision(6);

        entity.Property(e => e.LeadTicketCreationDate)
            .HasPrecision(6);

        entity.Property(e => e.OutsideBrokerName)
            .HasMaxLength(100);

        entity.Property(e => e.Phase)
            .HasMaxLength(100);

        entity.Property(e => e.PrimeTcrStatusName)
            .HasMaxLength(50)
            .HasColumnName("PrimeTcrStatusName");

        entity.Property(e => e.ProjectName)
            .HasMaxLength(100);

        entity.Property(e => e.PropertyType)
            .HasMaxLength(150);

        entity.Property(e => e.RecCloseDate)
            .HasPrecision(6);

        entity.Property(e => e.VerificationDate)
            .HasPrecision(6);

        entity.Property(e => e.RecReserveDate)
            .HasPrecision(6);

        entity.Property(e => e.Remarks)
            .HasMaxLength(2500);

        entity.Property(e => e.ResignedRuleSkippedIds)
            .HasMaxLength(255);

        entity.Property(e => e.SalesVolume)
            .HasPrecision(12, 2);

        entity.Property(e => e.SecondAgentForcedCommissioningPercentage)
            .HasPrecision(6, 5);

        entity.Property(e => e.SecondAgentName)
            .HasMaxLength(100);

        entity.Property(e => e.SecondAgentSharePercentage)
            .HasPrecision(6, 5);

        entity.Property(e => e.SecondAgentTreeJsonb)
            .HasColumnType("jsonb");

        entity.Property(e => e.SecondAgentTreeNames)
            .HasMaxLength(1000);

        entity.Property(e => e.SecondReferralAgentName)
            .HasMaxLength(100);

        entity.Property(e => e.SecondReferralAgentTreeJsonb)
            .HasColumnType("jsonb");

        entity.Property(e => e.SecondReferralAgentTreeNames)
            .HasMaxLength(1000);

        entity.Property(e => e.TaxPercentage)
            .HasPrecision(6, 5);

        entity.Property(e => e.ThirdAgentForcedCommissioningPercentage)
            .HasPrecision(6, 5);

        entity.Property(e => e.ThirdAgentName)
            .HasMaxLength(100);

        entity.Property(e => e.ThirdAgentSharePercentage)
            .HasPrecision(6, 5);

        entity.Property(e => e.ThirdAgentTreeJsonb)
            .HasColumnType("jsonb");

        entity.Property(e => e.ThirdAgentTreeNames)
            .HasMaxLength(1000);

        entity.Property(e => e.UnitNumber)
            .HasMaxLength(100);

        entity.Property(e => e.VerificationComment)
            .HasMaxLength(5000);
    }
}
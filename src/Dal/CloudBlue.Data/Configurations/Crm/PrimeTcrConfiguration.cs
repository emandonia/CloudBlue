using CloudBlue.Domain.DataModels.PrimeTcrs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CloudBlue.Data.Configurations.Crm;

internal class PrimeTcrConfiguration : IEntityTypeConfiguration<PrimeTcr>
{
    public void Configure(EntityTypeBuilder<PrimeTcr> entity)

    {
        entity.HasKey(e => e.Id)
            .HasName("PK__PrimeClo__C1784EA07D344E");

        entity.HasIndex(e => e.ClientId, "PrimeTcrs_ClientId_idx");

        entity.HasIndex(e => e.SalesVolume, "PrimeTcrs_SalesVolume_idx");

        entity.HasIndex(e => e.UnitNumberLowered, "PrimeTcrs_UnitNumberLowered_idx");

        entity.HasIndex(e => e.RecentEventsJsonb, "idx_PrimeTcrs_RecentEventsJsonb")
            .HasMethod("gin");

        entity.HasIndex(e => e.AgentsIdsArray, "idx_primetcrs_agentsidsarray")
            .HasMethod("gin");

        entity.HasIndex(e => e.ManagersIdsArray, "idx_primetcrs_managersidsarray")
            .HasMethod("gin");

        entity.Property(e => e.Id)
            .UseIdentityAlwaysColumn();

        entity.Property(e => e.AgentsIdsArray)
            ;

        entity.Property(e => e.BranchId)
            ;

        entity.Property(e => e.BuildUpArea)
            .HasPrecision(12, 2);

        entity.Property(e => e.ClientId)
            ;

        entity.Property(e => e.ClientName)
            .HasMaxLength(500);

        entity.Property(e => e.ClosingChannelExtraId)
            ;

        entity.Property(e => e.ClosingChannelId)
            ;

        entity.Property(e => e.CollectedCommissionValue)
            .HasPrecision(12, 2);

        entity.Property(e => e.CompanyCommissionPercentage)
            .HasPrecision(6, 5);

        entity.Property(e => e.CompanyCommissionValue)
            .HasPrecision(12, 2);

        entity.Property(e => e.CompanyId)
            ;

        entity.Property(e => e.ConfirmedContractingDate)
            .HasPrecision(6);

        entity.Property(e => e.ConfirmedContractingDateNumeric)
            ;

        entity.Property(e => e.ConfirmedHalfContractingDate)
            .HasPrecision(6);

        entity.Property(e => e.ConfirmedHalfContractingDateNumeric)
            ;

        entity.Property(e => e.ConfirmedReservingDate)
            .HasPrecision(6);

        entity.Property(e => e.ConfirmedReservingDateNumeric)
            ;

        entity.Property(e => e.ContractExpectedDate)
            .HasPrecision(6);

        entity.Property(e => e.CreatedBy)
            .HasMaxLength(100);

        entity.Property(e => e.CreatedById)
            ;

        entity.Property(e => e.CreationDateNumeric)
            ;

        entity.Property(e => e.CreationDateTime)
            .HasPrecision(6);

        entity.Property(e => e.CutOffId)
            ;

        entity.Property(e => e.DeveloperId)
            ;

        entity.Property(e => e.DeveloperName)
            .HasMaxLength(100);

        entity.Property(e => e.DocumentDate)
            .HasPrecision(6);

        entity.Property(e => e.DocumentTypeId)
            ;

        entity.Property(e => e.DueBalance)
            .HasPrecision(12, 2);

        entity.Property(e => e.ExtraManagerId)
            ;

        entity.Property(e => e.ExtraManagerName)
            .HasMaxLength(100);

        entity.Property(e => e.FirstAgentForcedCommissioningPercentage)
            .HasPrecision(6, 5);

        entity.Property(e => e.FirstAgentId)
            ;

        entity.Property(e => e.FirstAgentInResaleTeam)
            ;

        entity.Property(e => e.FirstAgentName)
            .HasMaxLength(100);

        entity.Property(e => e.FirstAgentPositionAtContractDateId)
            ;

        entity.Property(e => e.FirstAgentPositionAtHalfContractDateId)
            ;

        entity.Property(e => e.FirstAgentPositionId)
            ;

        entity.Property(e => e.FirstAgentPromoted)
            ;

        entity.Property(e => e.FirstAgentSharePercentage)
            .HasPrecision(6, 5);

        entity.Property(e => e.FirstAgentTreeJsonb)

            .HasColumnType("jsonb");

        entity.Property(e => e.FirstAgentTreeNames)
            .HasMaxLength(1000);

        entity.Property(e => e.FirstReferralAgentId)
            ;

        entity.Property(e => e.FirstReferralAgentInResaleTeam)
            ;

        entity.Property(e => e.FirstReferralAgentName)
            .HasMaxLength(100);

        entity.Property(e => e.FirstReferralAgentPositionId)
            ;

        entity.Property(e => e.FirstReferralAgentPromoted)
            ;

        entity.Property(e => e.FirstReferralAgentTreeJsonb)

            .HasColumnType("jsonb");

        entity.Property(e => e.FirstReferralAgentTreeNames)
            .HasMaxLength(1000);

        entity.Property(e => e.FirstReferralItemId)
            ;

        entity.Property(e => e.ForceAchievementPercentage)
            .HasPrecision(6, 5);

        entity.Property(e => e.ForceCommissionPercentage)
            .HasPrecision(6, 5);

        entity.Property(e => e.ForceFlatRateCommission)
            ;

        entity.Property(e => e.ForceHalfDeal)
            ;

        entity.Property(e => e.ForcedAgentIncentiveValue)
            .HasPrecision(12, 2);

        entity.Property(e => e.ForcedScaledCommissionPercentage)
            .HasPrecision(6, 5);

        entity.Property(e => e.FreezeCommissionAgentsIds)
            .HasMaxLength(255);

        entity.Property(e => e.FreezeCommissionManagersIds)
            .HasMaxLength(255);

        entity.Property(e => e.HalfCommissionCutOffId)
            ;

        entity.Property(e => e.HalfCommissionPaid)
            ;

        entity.Property(e => e.HaveDocument)
            ;

        entity.Property(e => e.IgnoreDebitedCommission)
            ;

        entity.Property(e => e.IgnoreDebitedIncentive)
            ;

        entity.Property(e => e.Invoiced)
            ;

        entity.Property(e => e.IsCompanyCommissionCollected)
            ;

        entity.Property(e => e.IsCorporate)
            ;

        entity.Property(e => e.IsDeleted)
            ;

        entity.Property(e => e.IsHalfCommission)
            ;

        entity.Property(e => e.IsHalfContracted)
            ;

        entity.Property(e => e.IsReOpen)
            ;

        entity.Property(e => e.IsRegular)
            ;

        entity.Property(e => e.IsResolved)
            ;

        entity.Property(e => e.LandArea)
            .HasPrecision(12, 2);

        entity.Property(e => e.LastConflictDate)
            .HasPrecision(6);

        entity.Property(e => e.LastConflictDateNumeric)
            ;

        entity.Property(e => e.LastDeveloperFeedBack)
            .HasMaxLength(2500);

        entity.Property(e => e.LastDeveloperFeedBackDate)
            .HasPrecision(6);

        entity.Property(e => e.LastDeveloperFeedBackDateNumeric)
            ;

        entity.Property(e => e.LastDeveloperReviewingDate)
            .HasPrecision(6);

        entity.Property(e => e.LastDeveloperReviewingDateNumeric)
            ;

        entity.Property(e => e.LastFeedBack)
            .HasMaxLength(5000);

        entity.Property(e => e.LastMarketingChannelExtraId)
            ;

        entity.Property(e => e.LastMarketingChannelId)
            ;

        entity.Property(e => e.LastPostponeDate)
            .HasPrecision(6);

        entity.Property(e => e.LastPostponeDateNumeric)
            ;

        entity.Property(e => e.LastReopenDate)
            .HasPrecision(6);

        entity.Property(e => e.LastReopenDateNumeric)
            ;

        entity.Property(e => e.LastResolveDate)
            .HasPrecision(6);

        entity.Property(e => e.LastResolveDateNumeric)
            ;

        entity.Property(e => e.LeadTicketId)
            ;

        entity.Property(e => e.ManagersIdsArray)
            ;

        entity.Property(e => e.OutsideBrokerCommissionPercentage)
            .HasPrecision(6, 5);

        entity.Property(e => e.OutsideBrokerCommissionValue)
            .HasPrecision(14, 4);

        entity.Property(e => e.OutsideBrokerId)
            ;

        entity.Property(e => e.OutsideBrokerName)
            .HasMaxLength(100);

        entity.Property(e => e.PendingStageId)
            ;

        entity.Property(e => e.Phase)
            .HasMaxLength(100);

        entity.Property(e => e.PrimeTcrStatusId)
            ;

        entity.Property(e => e.ProjectId)
            ;

        entity.Property(e => e.ProjectName)
            .HasMaxLength(100);

        entity.Property(e => e.PropertyTypeId)
            ;

        entity.Property(e => e.RecCloseDate)
            .HasPrecision(6);

        entity.Property(e => e.RecCloseDateNumeric)
            ;

        entity.Property(e => e.RecReserveDate)
            .HasPrecision(6);

        entity.Property(e => e.RecReserveDateNumeric)
            ;

        entity.Property(e => e.RecentEventsJsonb)

            .HasColumnType("jsonb");

        entity.Property(e => e.Remarks)
            .HasMaxLength(2500);

        entity.Property(e => e.ResignedRuleSkippedIds)
            .HasMaxLength(255);

        entity.Property(e => e.RestrictTargetCommission)
            ;

        entity.Property(e => e.SalesAccountingFeedBackId)
            ;

        entity.Property(e => e.SalesVolume)
            .HasPrecision(12, 2);

        entity.Property(e => e.SecondAgentForcedCommissioningPercentage)
            .HasPrecision(6, 5);

        entity.Property(e => e.SecondAgentId)
            ;

        entity.Property(e => e.SecondAgentInResaleTeam)
            ;

        entity.Property(e => e.SecondAgentName)
            .HasMaxLength(100);

        entity.Property(e => e.SecondAgentPositionAtContractDateId)
            ;

        entity.Property(e => e.SecondAgentPositionAtHalfContractDateId)
            ;

        entity.Property(e => e.SecondAgentPositionId)
            ;

        entity.Property(e => e.SecondAgentPromoted)
            ;

        entity.Property(e => e.SecondAgentSharePercentage)
            .HasPrecision(6, 5);

        entity.Property(e => e.SecondAgentTreeJsonb)

            .HasColumnType("jsonb");

        entity.Property(e => e.SecondAgentTreeNames)
            .HasMaxLength(1000);

        entity.Property(e => e.SecondReferralAgentId)
            ;

        entity.Property(e => e.SecondReferralAgentInResaleTeam)
            ;

        entity.Property(e => e.SecondReferralAgentName)
            .HasMaxLength(100);

        entity.Property(e => e.SecondReferralAgentPositionId)
            ;

        entity.Property(e => e.SecondReferralAgentPromoted)
            ;

        entity.Property(e => e.SecondReferralAgentTreeJsonb)

            .HasColumnType("jsonb");

        entity.Property(e => e.SecondReferralAgentTreeNames)
            .HasMaxLength(1000);

        entity.Property(e => e.SecondReferralItemId)
            ;

        entity.Property(e => e.SkipHalfCommissionRules)
            ;

        entity.Property(e => e.SkipIncentive)
            ;

        entity.Property(e => e.TaxPercentage)
            .HasPrecision(6, 5);

        entity.Property(e => e.TcrSelection)
            ;

        entity.Property(e => e.TeleSalesAgentId)
            ;

        entity.Property(e => e.TeleSalesAgentName)
            .HasMaxLength(255);

        entity.Property(e => e.ThirdAgentForcedCommissioningPercentage)
            .HasPrecision(6, 5);

        entity.Property(e => e.ThirdAgentId)
            ;

        entity.Property(e => e.ThirdAgentInResaleTeam)
            ;

        entity.Property(e => e.ThirdAgentName)
            .HasMaxLength(100);

        entity.Property(e => e.ThirdAgentPositionAtContractDateId)
            ;

        entity.Property(e => e.ThirdAgentPositionAtHalfContractDateId)
            ;

        entity.Property(e => e.ThirdAgentPositionId)
            ;

        entity.Property(e => e.ThirdAgentPromoted)
            ;

        entity.Property(e => e.ThirdAgentSharePercentage)
            .HasPrecision(6, 5);

        entity.Property(e => e.ThirdAgentTreeJsonb)

            .HasColumnType("jsonb");

        entity.Property(e => e.ThirdAgentTreeNames)
            .HasMaxLength(1000);

        entity.Property(e => e.UnitNumber)
            .HasMaxLength(100);

        entity.Property(e => e.UnitNumberLowered)
            .HasMaxLength(100);

        entity.Property(e => e.UsageId)
            ;

        entity.Property(e => e.VerificationComment)
            .HasMaxLength(5000);

        entity.Property(e => e.VerificationDate)
            .HasPrecision(6);

        entity.Property(e => e.VerificationStatusId)
            ;
    }
}
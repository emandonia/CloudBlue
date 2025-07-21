using CloudBlue.Domain.DataModels.Crm;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace CloudBlue.Data.Configurations.Crm;

internal class LeadTicketConfiguration : IEntityTypeConfiguration<LeadTicket>

{
    public void Configure(EntityTypeBuilder<LeadTicket> entity)
    {


        entity.HasKey(e => e.Id).HasName("LeadTickets_pkey");

        entity.ToTable("LeadTickets");

        entity.HasIndex(e => e.ActiveCallLaterCount, "LeadTickets_ActiveCallLaterCount_idx");

        entity.HasIndex(e => e.ActiveRemindersCount, "LeadTickets_ActiveRemindersCount_idx");

        entity.HasIndex(e => e.AgentLastEventContactingTypeId, "LeadTickets_AgentLastEventContactingTypeId_idx");

        entity.HasIndex(e => e.AgentLastEventCreationDateTimeNumeric, "LeadTickets_AgentLastEventCreationDateTimeNumeric_idx");

        entity.HasIndex(e => e.BranchId, "LeadTickets_BranchId_idx");

        entity.HasIndex(e => e.CallId, "LeadTickets_CallId_idx");

        entity.HasIndex(e => e.CallLaterCount, "LeadTickets_CallLaterCount_idx");

        entity.HasIndex(e => e.CallLaterDistinctCount, "LeadTickets_CallLaterDistinctCount_idx");

        entity.HasIndex(e => e.ClientId, "LeadTickets_ClientId_idx");

        entity.HasIndex(e => e.CompanyId, "LeadTickets_CompanyId_idx");

        entity.HasIndex(e => e.CreatedById, "LeadTickets_CreatedById_idx");

        entity.HasIndex(e => e.CreationDateNumeric, "LeadTickets_CreationDateNumeric_idx");

        entity.HasIndex(e => e.CurrentAgentId, "LeadTickets_CurrentAgentId_idx");

        entity.HasIndex(e => e.DistrictId, "LeadTickets_DistrictId_idx");

        entity.HasIndex(e => e.FormName, "LeadTickets_FormName_idx");

        entity.HasIndex(e => e.HoursToGetInProgress, "LeadTickets_HoursToGetInProgress_idx");

        entity.HasIndex(e => new { e.IsClosed, e.IsArchived, e.IsFullLeadTicket }, "LeadTickets_IsClosed_IsArchived_IsFullLeadTicket_idx");

        entity.HasIndex(e => e.KnowSourceExtraId, "LeadTickets_KnowSourceExtraId_idx");

        entity.HasIndex(e => e.KnowSourceId, "LeadTickets_KnowSourceId_idx");

        entity.HasIndex(e => e.LastAssignedDateNumeric, "LeadTickets_LastAssignedDateNumeric_idx");

        entity.HasIndex(e => e.LeadTicketStatusId, "LeadTickets_LeadTicketStatusId_idx");

        entity.HasIndex(e => e.MarketingAgencyId, "LeadTickets_MarketingAgencyId_idx");

        entity.HasIndex(e => e.NeighborhoodId, "LeadTickets_NeighborhoodId_idx");

        entity.HasIndex(e => e.NoAnswerCount, "LeadTickets_NoAnswerCount_idx");

        entity.HasIndex(e => e.NoAnswerDistinctCount, "LeadTickets_NoAnswerDistinctCount_idx");

        entity.HasIndex(e => e.PropertyTypeId, "LeadTickets_PropertyTypeId_idx");

        entity.HasIndex(e => e.QualifiedCount, "LeadTickets_QualifiedCount_idx");

        entity.HasIndex(e => e.QualifiedDistinctCount, "LeadTickets_QualifiedDistinctCount_idx");

        entity.HasIndex(e => e.ReassignCount, "LeadTickets_ReassignCount_idx");

        entity.HasIndex(e => e.ReassignedNewOnce, "LeadTickets_ReassignedNewOnce_idx");

        entity.HasIndex(e => e.RemindersCount, "LeadTickets_RemindersCount_idx");

        entity.HasIndex(e => e.RemindersDistinctCount, "LeadTickets_RemindersDistinctCount_idx");

        entity.HasIndex(e => e.SalesTypeId, "LeadTickets_SalesTypeId_idx");

        entity.HasIndex(e => e.ServiceId, "LeadTickets_ServiceId_idx");

        entity.HasIndex(e => e.SetInProgressDateNumeric, "LeadTickets_SetInProgressDateNumeric_idx");

        entity.HasIndex(e => e.SettingInProgressCount, "LeadTickets_SettingInProgressCount_idx");

        entity.HasIndex(e => e.LeadSourceId, "LeadTickets_SourceId_idx");

        entity.HasIndex(e => e.UnQualifiedCount, "LeadTickets_UnQualifiedCount_idx");

        entity.HasIndex(e => e.UnQualifiedDistinctCount, "LeadTickets_UnQualifiedDistinctCount_idx");

        entity.HasIndex(e => e.UsageId, "LeadTickets_UsageId_idx");

        entity.HasIndex(e => e.AlreadyExistCount, "i_leadtickets_AlreadExistCount").HasNullSortOrder(new[] { NullSortOrder.NullsFirst });

        entity.HasIndex(e => e.ActivityStatsJsonb, "idx_leadtickets_ActivityStatsJsonb").HasMethod("gin");

        entity.Property(e => e.Id).UseIdentityAlwaysColumn();
        entity.Property(e => e.ActiveCallLaterCount);
        entity.Property(e => e.ActiveRemindersCount);
        entity.Property(e => e.ActivityStatsJsonb)

            .HasColumnType("jsonb");
        entity.Property(e => e.AdLink).HasMaxLength(400);
        entity.Property(e => e.AgencyAbbrev).HasMaxLength(255);
        entity.Property(e => e.AgentFirstEventIdAfterAssign);
        entity.Property(e => e.AgentLastEventComment).HasMaxLength(1250);
        entity.Property(e => e.AgentLastEventContactingTypeId);
        entity.Property(e => e.AgentLastEventCreationDateTimeNumeric);
        entity.Property(e => e.AgentLastEventDateTimeNumeric);
        entity.Property(e => e.AgentLastEventId);
        entity.Property(e => e.AgentLastEventTypeId);
        entity.Property(e => e.AlreadyExistCount);
        entity.Property(e => e.ApplyCampaignOwnerShipRules);
        entity.Property(e => e.ApplyTwentyFourHoursRules);
        entity.Property(e => e.BranchId);
        entity.Property(e => e.BudgetFrom).HasPrecision(18, 2);
        entity.Property(e => e.BudgetTo).HasPrecision(18, 2);
        entity.Property(e => e.CallBackDateNumeric);
        entity.Property(e => e.CallId);
        entity.Property(e => e.CallLaterCount);
        entity.Property(e => e.CallLaterDistinctCount);
        entity.Property(e => e.CampaignOwnerId);
        entity.Property(e => e.ClientId);
        entity.Property(e => e.CompanyId);
        entity.Property(e => e.CorporateCompanyId);
        entity.Property(e => e.CreatedBy).HasMaxLength(120);
        entity.Property(e => e.CreatedById);
        entity.Property(e => e.CreationDate).HasPrecision(6);
        entity.Property(e => e.CreationDateNumeric);
        entity.Property(e => e.CurrencyId);
        entity.Property(e => e.CurrentAgentId);
        entity.Property(e => e.DateVoided).HasPrecision(6);
        entity.Property(e => e.DistrictId);
        entity.Property(e => e.ExtendedDateNumeric);
        entity.Property(e => e.FirstOwnerId);
        entity.Property(e => e.FormName).HasMaxLength(1200);
        entity.Property(e => e.HoursToGetInProgress);
        entity.Property(e => e.IsArchived);
        entity.Property(e => e.IsClosed);
        entity.Property(e => e.IsDuplicated);
        entity.Property(e => e.IsFullLeadTicket);
        entity.Property(e => e.IsOld);
        entity.Property(e => e.IsOptedOut);
        entity.Property(e => e.IsPotential);
        entity.Property(e => e.IsVip);
        entity.Property(e => e.IsVoided);
        entity.Property(e => e.KnowSourceExtraId);
        entity.Property(e => e.KnowSourceId);
        entity.Property(e => e.LastAssignedDateNumeric);
        entity.Property(e => e.LastEventComment).HasMaxLength(1200);
        entity.Property(e => e.LastEventCreationDateTimeNumeric);
        entity.Property(e => e.LastEventId);
        entity.Property(e => e.LastEventProcessId);
        entity.Property(e => e.LeadSourceId);
        entity.Property(e => e.LeadTicketNote).HasMaxLength(2500);
        entity.Property(e => e.LeadTicketStatusId);
        entity.Property(e => e.Location).HasMaxLength(1200);
        entity.Property(e => e.MarketingAgencyId);
        entity.Property(e => e.NeighborhoodId);
        entity.Property(e => e.NoAnswerCount);
        entity.Property(e => e.NoAnswerDistinctCount);
        entity.Property(e => e.OriginalProject).HasMaxLength(255);
        entity.Property(e => e.OriginalProjectId);
        entity.Property(e => e.PendingAlreadyExistView);
        entity.Property(e => e.PropertyTypeId);
        entity.Property(e => e.ProspectRequestStatusId);
        entity.Property(e => e.QualifiedCount);
        entity.Property(e => e.QualifiedDistinctCount);
        entity.Property(e => e.ReassignCount);
        entity.Property(e => e.ReassignedNewOnce);
        entity.Property(e => e.ReassignedOnce);
        entity.Property(e => e.RecentEventsJsonb)

            .HasColumnType("jsonb");
        entity.Property(e => e.ReferralId);
        entity.Property(e => e.RejectReasonId);
        entity.Property(e => e.RemindersCount);
        entity.Property(e => e.RemindersDistinctCount);
        entity.Property(e => e.SalesTypeId);
        entity.Property(e => e.ServiceId);
        entity.Property(e => e.SetInProgressDateNumeric);
        entity.Property(e => e.SettingInProgressCount);
        entity.Property(e => e.TcrStatusId);
        entity.Property(e => e.UnQualifiedCount);
        entity.Property(e => e.UnQualifiedDistinctCount);
        entity.Property(e => e.UsageId);
        entity.Property(e => e.ViewedByCurAgent);
        entity.Property(e => e.VoidReasonId);
        entity.Property(e => e.VoidingReason).HasMaxLength(1200);
        entity.Property(e => e.WasOld);
        entity.Property(e => e.WebLeadId);
        entity.Property(e => e.WrongNumberAction);

        entity.HasOne(z => z.LeadTicketExtension)
            .WithOne(z => z.LeadTicket)
            .HasForeignKey<LeadTicketExtension>(lte => lte.LeadTicketId)
            .HasPrincipalKey<LeadTicket>(z => z.Id);
    }
}
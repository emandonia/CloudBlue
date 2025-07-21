using CloudBlue.Domain.DataModels.Crm;
using CloudBlue.Domain.DomainModels;
using CloudBlue.Domain.DomainModels.CallLeads;
using CloudBlue.Domain.DomainModels.Filtration.Interfaces;
using CloudBlue.Domain.DomainModels.Filtration.JsonFilters;
using CloudBlue.Domain.Enums;
using CloudBlue.Domain.GenericTypes;
using CloudBlue.Domain.Interfaces.DbContext;
using CloudBlue.Domain.Interfaces.Repositories;
using CloudBlue.Domain.Interfaces.Services;
using CloudBlue.Domain.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace CloudBlue.Data.Repositories.Crm;

public class LeadTicketsRepository(ICrmDataContext appDb) : ILeadTicketsRepository
{
    public int CurrentUserId { get; set; }
    public int CurrentUserBranchId { get; set; }
    public int CurrentUserCompanyId { get; set; }

    public long LastCreatedItemId { get; set; }

    public async Task<bool> CreateLeadTicketAsync(LeadTicketCreateModel model)
    {
        var mainLead = new LeadTicket
        {
            ClientId = model.ClientId,
            BranchId = model.BranchId,
            CompanyId = model.CompanyId,
            LeadTicketNote = model.Comment ?? string.Empty,

            // Mapping nested Location object
            DistrictId = model.Location.DistrictId,
            NeighborhoodId = model.Location.NeighborhoodId,
            OriginalProject = model.Location.ProjectName,
            OriginalProjectId = model.Location.NeighborhoodId,
            Location = model.LocationStr ?? string.Empty,

            // Set default values or initialize specific fields as required
            LeadTicketStatusId = model.LeadTicketStatusId,
            BudgetFrom = model.ClientBudget.BudgetFrom,
            BudgetTo = model.ClientBudget.BudgetTo,
            CurrencyId = model.ClientBudget.CurrencyId,
            ServiceId = model.ServiceId,
            CurrentAgentId = model.AgentId,
            IsClosed = false,
            CreatedById = CurrentUserId,
            CreationDate = DateTime.UtcNow,
            CreationDateNumeric = int.Parse(DateTime.UtcNow.ToString("yyyyMMdd")),
            LeadSourceId = model.LeadSourceInfo.LeadSourceId,
            KnowSourceId = model.LeadSourceInfo.KnowSourceId,
            CallId = model.CallId,
            KnowSourceExtraId = model.LeadSourceInfo.KnowSubSourceId,
            IsArchived = false,
            IsFullLeadTicket = model.IsFullLeadTicket,
            ReferralId = model.ReferralId,
            IsOld = false,
            PendingAlreadyExistView = model.PendingAlreadyExistView,
            HoursToGetInProgress = 0,
            TcrStatusId = 0,
            TcrTypeId = 0,
            ApplyCampaignOwnerShipRules = model.ApplyCampaignOwnerShipRules,
            ApplyTwentyFourHoursRules = model.ApplyTwentyFourHoursRules,
            CampaignOwnerId = 0,
            CollectiveCampaignId = 0,
            ProjectCampaignId = 0,
            IsVoided = false,
            VoidingReason = string.Empty,
            DateVoided = null,
            FormName = model.LeadSourceInfo.DigitalFormName,
            MarketingAgencyId = model.LeadSourceInfo.MarketingAgencyId,
            AdLink = string.Empty,
            LastAssignedDateNumeric =
                model.LastAssignedDate.HasValue ? int.Parse(model.LastAssignedDate.Value.ToString("yyyyMMdd")) : 0,
            LastAssignedDate = model.LastAssignedDate,
            LastEventId = 0,
            AgentLastEventId = 0,
            AgentLastEventCreationDateTimeNumeric = 0,
            AgentLastEventTypeId = 0,
            AgentLastEventDateTimeNumeric = 0,
            AgentLastEventContactingTypeId = 0,
            AgentLastEventDateTime = null,
            LastEventComment = string.Empty,
            CallBackDate = null,
            CallBackDateNumeric = 0,
            ExtendedDate = null,
            ExtendedDateNumeric = 0,
            WrongNumberAction = false,
            IsDuplicated = false,
            IsVip = false,
            IsOptedOut = false,
            IsPotential = false,
            ViewedByCurAgent = false,
            AgentFirstEventIdAfterAssign = 0,
            CorporateCompanyId = model.CorporateCompanyId,
            CreatedBy = model.CurrentUserName,
            LastEventProcessId = 0,
            PropertyTypeId = model.PropertyTypeId,
            ProspectRequestStatusId = 0,
            SalesTypeId = model.SalesTypeId,
            UsageId = model.UsageId,
            AlreadyExistCount = 0,
            FirstOwnerId = model.AgentId,
            ReassignCount = 0,
            ReassignedNewOnce = 0,
            ReassignedOnce = false,
            SetInProgressDateNumeric = 0,
            WebLeadId = 0,
            SettingInProgressCount = 0,
            LastEventCreationDateTimeNumeric = 0,
            AgencyAbbrev = string.Empty,
            LastEventCreationDateTime = null,
            WasOld = false,
            CallLaterCount = 0,
            CallLaterDistinctCount = 0,
            NoAnswerCount = 0,
            UnQualifiedDistinctCount = 0,
            UnQualifiedCount = 0,
            QualifiedDistinctCount = 0,
            QualifiedCount = 0,
            NoAnswerDistinctCount = 0,
            AgentLastEventCreationDateTime = null,
            AgentLastEventComment = string.Empty,
            LeadTicketExtension = new LeadTicketExtension
            {
                AssociatedLeadId = 0,
                ClientNameUpdated = false,
                ConvertedFromCall = model.CallId > 0,
                ConvertedFromDummy = false,
                ConvertedFromReferral = model.ReferralId > 0,
                CurrentAgentManagersTree = string.Empty,
                DuplicateLeadId = 0,
                ExpRegId = 0,
                ExtendedStatusId = 0,
                IsCorporate = model.IsCorporate == 1,
                LastConversionEventId = 0,
                IsPureOld = false,
                IsReserved = false,
                LastDeactivatedDate = null,
                LastDeactivatedDateNumeric = 0,
                LastRevivalDate = null,
                LastRevivalDateNumeric = 0,
                LastVoidedDate = null,
                LastVoidedDateNumeric = 0,
                OldAgentId = 0,
                OldStatusId = 0,
                RejectDate = null,
                RejectReason = string.Empty,
                OldLeadRefId = 0,
                RevivedByAgentId = 0,
                RevivedById = 0,
                SalesForceLeadId = 0,
                SettingOldLogId = 0,
                ShowDummyToAgent = false,
                TeleSalesAgentId = 0,
                VoidingLeadTicketId = 0
            }
        };

        await appDb.LeadTickets.AddAsync(mainLead);
        await appDb.SaveChangesAsync();
        LastCreatedItemId = mainLead.Id;

        return true;
    }

    public async Task<LeadTicketInfoItemForTcr?> GetLeadTicketForPrimeTcrAsync(long id)
    {
        return await appDb.VwLeadTickets.Where(z => z.Id == id && z.IsFullLeadTicket)
            .Select(z => new LeadTicketInfoItemForTcr
            {
                AgentName = z.CurrentAgent,
                AgentId = z.CurrentAgentId,
                CompanyId = z.CompanyId,
                BranchId = z.BranchId,
                ClientName = z.ClientName,
                CompanyName = z.CompanyName,
                UsageId = z.UsageId,
                Usage = z.Usage,
                BranchName = z.BranchName,
                ClientId = z.ClientId,
                SalesTypeId = z.SalesTypeId,
                LeadTicketId = z.Id,
                LeadTicketStatusId = z.LeadTicketStatusId,
                PropertyTypeId = z.PropertyTypeId,
                ServiceTypeId = z.ServiceId,
                KnowSource = z.KnowSource,
                KnowSourceId = z.KnowSourceId,
                KnowSubSource = z.KnowSubSource ?? "N/A",
                KnowSubSourceId = z.KnowSourceExtraId,
                IsCorporate = z.CorporateCompanyId > 0,
                LeadSource = z.LeadSource,
                TeleSalesAgentName = z.TeleSalesAgentName,
                TeleSalesAgentId = z.TeleSalesAgentId
            })
            .FirstOrDefaultAsync();
    }

    public async Task<ListResult<LeadTicketItemForList>> GetLeadTicketsAsync(LeadTicketsFiltersModel filters,
        ILookUpsService lookUpsService)
    {
        var rawLeads = appDb.VwLeadTickets.AsQueryable();
        #region Filters

        if (string.IsNullOrEmpty(filters.ExtraFilters) == false)
        {
            rawLeads = rawLeads.Where(filters.ExtraFilters);
        }

        #region Client

        if (string.IsNullOrEmpty(filters.ClientName) == false)
        {
            rawLeads = rawLeads.Where(z =>
                string.IsNullOrEmpty(z.ClientNameLowered) == false &&
                EF.Functions.Like(z.ClientNameLowered, $"%{filters.ClientName.ToLower()}%"));
        }

        if (string.IsNullOrEmpty(filters.ClientNameArabic) == false)
        {
            rawLeads = rawLeads.Where(z =>
                z.ClientNameArabic != null && z.ClientNameArabic == filters.ClientNameArabic);
        }

        if (string.IsNullOrEmpty(filters.ClientContactDevice) == false)
        {
            var jsonFilter = new[] { new ClientDeviceInfoFilter(filters.ClientContactDevice) };
            var deviceFilter = UtilityFunctions.SerializeToJsonString(jsonFilter);

            rawLeads = rawLeads.Where(e =>
                e.ContactDevicesJsonb != null && EF.Functions.JsonContains(e.ContactDevicesJsonb, deviceFilter));
        }

        if (string.IsNullOrEmpty(filters.CountryCode) == false)
        {
            var jsonFilter = new[] { new ClientCountryInfoFilter(filters.CountryCode) };
            var countryFilter = UtilityFunctions.SerializeToJsonString(jsonFilter);

            rawLeads = rawLeads.Where(e =>
                e.ContactDevicesJsonb != null && EF.Functions.JsonContains(e.ContactDevicesJsonb, countryFilter));
        }

        if (filters.InternationalOnly > 0)
        {
            var flag = filters.InternationalOnly != 1;
            var jsonFilter = new[] { new ClientCountryInfoFilter("0020") };
            var countryFilter = UtilityFunctions.SerializeToJsonString(jsonFilter);

            rawLeads = rawLeads.Where(e =>
                e.ContactDevicesJsonb != null &&
                EF.Functions.JsonContains(e.ContactDevicesJsonb, countryFilter) == flag);
        }

        if (filters.ClientCategoryId > 0)
        {
            rawLeads = rawLeads.Where(z => z.ClientCategoryId == filters.ClientCategoryId);
        }

        #endregion

        #region Location

        if (filters.NeighborhoodId > 0)
        {
            rawLeads = rawLeads.Where(z => z.NeighborhoodId == filters.NeighborhoodId);
        }
        else if (filters.DistrictId > 0)
        {
            rawLeads = rawLeads.Where(z => z.DistrictId == filters.DistrictId);
        }
        else if (filters.CityId > 0)
        {
            rawLeads = rawLeads.Where(z => z.CityId == filters.CityId);
        }
        else if (filters.CountryId > 0)
        {
            rawLeads = rawLeads.Where(z => z.CountryId == filters.CountryId);
        }

        #endregion

        #region Lead Source

        if (filters.LeadSourceId > 0)
        {
            rawLeads = rawLeads.Where(z => z.LeadSourceId == filters.LeadSourceId);
        }

        if (filters.AgencyId > 0)
        {
            rawLeads = rawLeads.Where(z => z.MarketingAgencyId == filters.AgencyId);
        }

        if (string.IsNullOrEmpty(filters.FormName) == false)
        {
            rawLeads = rawLeads.Where(z => z.FormName != null && z.FormName.ToLower() == filters.FormName.ToLower());
        }

        if (filters.KnowSubSourceId > 0)
        {
            rawLeads = rawLeads.Where(z => z.KnowSourceExtraId == filters.KnowSubSourceId);
        }
        else if (filters.KnowSourceId > 0)
        {
            rawLeads = rawLeads.Where(z => z.KnowSourceId == filters.KnowSourceId);
        }

        #endregion

        #region Lead ticket
        if (filters.AgentResignDateFrom != null)
        {
            var dateNumeric = int.Parse(filters.AgentResignDateFrom.Value.ToString("yyyyMMdd"));
            rawLeads = rawLeads.Where(z => z.ResignDateNumeric > 0 && z.ResignDateNumeric >= dateNumeric);
        }

        if (filters.AgentResignDateTo != null)
        {
            var dateNumeric = int.Parse(filters.AgentResignDateTo.Value.ToString("yyyyMMdd"));
            rawLeads = rawLeads.Where(z => z.ResignDateNumeric > 0 && z.ResignDateNumeric <= dateNumeric);
        }

        if (filters.CompanyId > 0)
        {
            rawLeads = rawLeads.Where(z => z.CompanyId == filters.CompanyId);
        }

        if (filters.BranchId > 0)
        {
            rawLeads = rawLeads.Where(z => z.BranchId == filters.BranchId);
        }

        if (string.IsNullOrEmpty(filters.EntityIds) == false)
        {
            var entityIds = UtilityFunctions.GetLongListFromString(filters.EntityIds)
                .ToArray();

            if (entityIds.Any())
            {
                rawLeads = rawLeads.Where(z => entityIds.Contains(z.Id));
            }
        }

        if (filters.ArchivedId > 0)
        {
            var archived = filters.ArchivedId == 1;
            rawLeads = rawLeads.Where(z => z.IsArchived == archived);
        }

        if (filters.FeedbackIds.Any())
        {
            if (filters.FeedbackIds.Count() == 1)
            {
                if (filters.LastFeedbackOnly)
                {
                    rawLeads = rawLeads.Where(z => z.LastAgentFeedBackId == filters.FeedbackIds.First());
                }
                else
                {
                    var jsonFilter = new[] { new AgentFeedbackFilter(filters.FeedbackIds.First()) };
                    var feedbackFilter = UtilityFunctions.SerializeToJsonString(jsonFilter);

                    rawLeads = rawLeads.Where(e =>
                        e.ActivityStatsJsonb != null && EF.Functions.JsonContains(e.ActivityStatsJsonb, feedbackFilter));
                }

            }
            else
            {
                rawLeads = rawLeads.Where(z => filters.FeedbackIds.Contains(z.LastAgentFeedBackId));

            }
        }

        if (filters.PendingActivityId > 0)
        {
            if (filters.PendingActivityId == (int)ContactingTypes.CallLater)
            {
                rawLeads = rawLeads.Where(e => e.ActiveCallLaterCount > 0);
            }
            else
            {
                rawLeads = rawLeads.Where(e => e.ActiveRemindersCount > 0);
            }
        }

        if ((filters.EntityStatusIds.Any() && filters.ExtremeHours == 0) || filters.EntityStatusIds.Count() > 1)
        {
            rawLeads = rawLeads.Where(z => filters.EntityStatusIds.ToArray()
                .Contains(z.LeadTicketStatusId));
        }
        else if (filters.EntityStatusIds.Count() == 1 && filters.ExtremeHours > 0)
        {
            var statusId = filters.EntityStatusIds.First();
            var status = (LeadTicketStatuses)statusId;
            var twoHoursAgo = DateTimeOffset.UtcNow.AddHours(-1 * filters.ExtremeHours);

            if (status == LeadTicketStatuses.Assigned)
            {
                if (filters.ReverseAssignDateComparison)
                {
                    rawLeads = rawLeads.Where(z =>
                    z.LeadTicketStatusId == statusId && z.LastAssignedDate != null &&
                    z.LastAssignedDate.Value > twoHoursAgo);

                }
                else
                {
                    rawLeads = rawLeads.Where(z =>
                   z.LeadTicketStatusId == statusId && z.LastAssignedDate != null &&
                   z.LastAssignedDate.Value <= twoHoursAgo);
                }

            }
            else if (status == LeadTicketStatuses.InProgress)
            {
                rawLeads = rawLeads.Where(z =>
                    z.LeadTicketStatusId == statusId && z.SetInProgressDate != null &&
                    z.SetInProgressDate.Value <= twoHoursAgo);
            }
        }

        if (filters.EntityCreationDateFrom != null)
        {
            var dateNumeric = int.Parse(filters.EntityCreationDateFrom.Value.ToString("yyyyMMdd"));
            rawLeads = rawLeads.Where(z => z.CreationDateNumeric > 0 && z.CreationDateNumeric >= dateNumeric);
        }

        if (filters.EntityCreationDateTo != null)
        {
            var dateNumeric = int.Parse(filters.EntityCreationDateTo.Value.ToString("yyyyMMdd"));
            rawLeads = rawLeads.Where(z => z.CreationDateNumeric > 0 && z.CreationDateNumeric <= dateNumeric);
        }

        if (filters.EntityAssignDateFrom != null)
        {
            var dateNumeric = int.Parse(filters.EntityAssignDateFrom.Value.ToString("yyyyMMdd"));
            rawLeads = rawLeads.Where(z => z.LastAssignedDateNumeric > 0 && z.LastAssignedDateNumeric >= dateNumeric);
        }

        if (filters.EntityAssignDateTo != null)
        {
            var dateNumeric = int.Parse(filters.EntityAssignDateTo.Value.ToString("yyyyMMdd"));
            rawLeads = rawLeads.Where(z => z.LastAssignedDateNumeric > 0 && z.LastAssignedDateNumeric <= dateNumeric);
        }

        if (filters.AgentLastActivityDateFrom != null)
        {
            var dateNumeric = long.Parse(filters.AgentLastActivityDateFrom.Value.ToString("yyyyMMdd"));

            rawLeads = rawLeads.Where(z =>
                z.AgentLastEventCreationDateTimeNumeric > 0 &&
                ((z.AgentLastEventCreationDateTimeNumeric < 21000000 &&
                  z.AgentLastEventCreationDateTimeNumeric >= dateNumeric) ||
                 (z.AgentLastEventCreationDateTimeNumeric > 21000000 &&
                  z.AgentLastEventCreationDateTimeNumeric >= dateNumeric * 1000000)));
        }

        if (filters.AgentLastActivityDateTo != null)
        {
            var dateNumeric = long.Parse(filters.AgentLastActivityDateTo.Value.ToString("yyyyMMdd"));

            rawLeads = rawLeads.Where(z =>
                z.AgentLastEventCreationDateTimeNumeric > 0 &&
                ((z.AgentLastEventCreationDateTimeNumeric < 21000000 &&
                  z.AgentLastEventCreationDateTimeNumeric <= dateNumeric) ||
                 (z.AgentLastEventCreationDateTimeNumeric > 21000000 &&
                  z.AgentLastEventCreationDateTimeNumeric <= dateNumeric * 1000000)));
        }

        if (filters.AssigningType != null)
        {
            int feedBackId;
            long dateValue;
            switch (filters.AssigningType)
            {
                case AssigningTypes.FreshFirstAgent:
                    rawLeads = rawLeads.Where(z => z.ReassignCount == 0 && z.ReassignedNewOnce == 0);

                    break;

                case AssigningTypes.FreshReAssigned:
                    rawLeads = rawLeads.Where(z => z.ReassignedNewOnce > 0 && z.ReassignedNewOnce >= z.ReassignCount);

                    break;

                case AssigningTypes.AllFreshLeads:
                    rawLeads = rawLeads.Where(z => ((z.ReassignedNewOnce > 0 && z.ReassignedNewOnce >= z.ReassignCount) || (z.ReassignCount == 0 && z.ReassignedNewOnce == 0)));

                    break;





                case AssigningTypes.ReAssigned:
                    rawLeads = rawLeads.Where(z => z.ReassignCount > 0 && z.ReassignCount > z.ReassignedNewOnce);

                    break;

                case AssigningTypes.ReassignedOnce:
                    rawLeads = rawLeads.Where(z => z.ReassignedOnce == true || z.ReassignCount > 0);

                    break;

                case AssigningTypes.QualifiedOnce:
                    rawLeads = rawLeads.Where(z => z.QualifiedCount > 0);

                    break;

                case AssigningTypes.QualifiedLeadsExceedTwoWeeks:
                    feedBackId = (int)ContactingTypes.Qualified;
                    dateValue = long.Parse(DateTime.UtcNow.AddDays(-14).ToString("yyyyMMddHHmmss"));

                    rawLeads = rawLeads.Where(z => (z.LastAgentFeedBackId == feedBackId && (z.LastAgentFeedBackDateNumeric == 0 || z.LastAgentFeedBackDateNumeric < dateValue)));

                    break;

                case AssigningTypes.AssignedLeadsExceedTwoHours:
                    var assignedId = (int)LeadTicketStatuses.Assigned;

                    var twoHoursAgo = DateTimeOffset.UtcNow.AddHours(-2);


                    rawLeads = rawLeads.Where(z =>
                    z.LeadTicketStatusId == assignedId && z.LastAssignedDate != null &&
                    z.LastAssignedDate.Value > twoHoursAgo);

                    break;
                case AssigningTypes.NoAnswerLeadsExceedTwoHours:
                    feedBackId = (int)ContactingTypes.NoAnswer;
                    dateValue = long.Parse(DateTime.UtcNow.AddHours(-2).ToString("yyyyMMddHHmmss"));

                    rawLeads = rawLeads.Where(z => (z.LastAgentFeedBackId == feedBackId && (z.LastAgentFeedBackDateNumeric == 0 || z.LastAgentFeedBackDateNumeric < dateValue)));

                    break;
                case AssigningTypes.CallLaterLeadsExceedFollowUpDate:
                    feedBackId = (int)ContactingTypes.CallLater;
                    var now = DateTimeOffset.UtcNow;


                    rawLeads = rawLeads.Where(z => (z.LastAgentFeedBackId == feedBackId && (z.CallBackDate == null || z.CallBackDate < now)));

                    break;







            }
        }


        if (filters.ManagersIds.Any())
        {

            rawLeads = rawLeads.Where(z => (z.ManagersIdsArray != null && z.ManagersIdsArray.Any(x => filters.ManagersIds.Contains(x))));




        }

        if (filters.AgentsIds.Any())
        {
            rawLeads = rawLeads.Where(
                z => filters.AgentsIds.Contains(z.CurrentAgentId));

        }
        if (filters.TopManagerId > 0)
        {
            rawLeads = rawLeads.Where(z => (z.ManagersIdsArray != null && z.ManagersIdsArray.Any(x => x == filters.TopManagerId)));

        }
        if (filters.DirectManagerId > 0)
        {
            rawLeads = rawLeads.Where(z => (z.ManagersIdsArray != null && z.ManagersIdsArray.Any(x => x == filters.DirectManagerId)));

        }
        if (filters.PropertyTypeId > 0)
        {
            rawLeads = rawLeads.Where(z => z.PropertyTypeId == filters.PropertyTypeId);
        }
        else if (filters.UsageId > 0)
        {
            rawLeads = rawLeads.Where(z => z.UsageId == filters.UsageId);
        }

        if (filters.SalesTypeId > 0)
        {
            rawLeads = rawLeads.Where(z => z.SalesTypeId == filters.SalesTypeId);
        }

        if (filters.ServiceId > 0)
        {
            rawLeads = rawLeads.Where(z => z.ServiceId == filters.ServiceId);
        }

        if (filters.BudgetFrom > 0)
        {
            rawLeads = rawLeads.Where(z => z.BudgetFrom >= filters.BudgetFrom);
        }

        if (filters.BudgetTo > 0)
        {
            rawLeads = rawLeads.Where(z => z.BudgetTo <= filters.BudgetTo);
        }

        if (filters.CurrencyId > 0)
        {
            rawLeads = rawLeads.Where(z => z.CurrencyId == filters.CurrencyId);
        }

        #endregion

        #endregion
        appDb.SetHighTimeOut();
        var retObj = new ListResult<LeadTicketItemForList>();
        retObj.TotalCount = await rawLeads.CountAsync();

        if (filters.ExportMode == false)
        {
            rawLeads = await PopulateSorting(rawLeads, filters, lookUpsService);
        }

        if (filters.ExportMode)
        {
            filters.PageSize = retObj.TotalCount;

        }


        var rawItems = await rawLeads.Skip(filters.PageIndex * filters.PageSize)
            .Take(filters.PageSize)
            .AsNoTracking()
            .ToArrayAsync();

        var items = new List<LeadTicketItemForList>();

        foreach (var rawItem in rawItems)
        {
            var item = new LeadTicketItemForList
            {
                Id = rawItem.Id,
                CreationDate = rawItem.CreationDate,
                AgentPosition = rawItem.AgentPosition ?? "N/A",
                BranchName = rawItem.BranchName ?? "N/A",
                ClientName = rawItem.ClientName,
                ClientNameArabic = rawItem.ClientNameArabic ?? "N/A",
                Location = rawItem.Location ?? "N/A",
                CompanyName = rawItem.CompanyName ?? "N/A",
                CreatedBy = rawItem.CreatedBy ?? "N/A",
                Usage = rawItem.Usage ?? "N/A",
                SalesType = rawItem.SalesType ?? "N/A",
                ClientId = rawItem.ClientId,
                IsArchived = rawItem.IsArchived,
                KnowSource = rawItem.KnowSource ?? "N/A",
                KnowSubSource = rawItem.KnowSubSource ?? "N/A",
                Country = rawItem.Country ?? "N/A",
                City = rawItem.City ?? "N/A",
                District = rawItem.District ?? "N/A",
                Project = rawItem.Project ?? "N/A",
                LeadSource = rawItem.LeadSource ?? "N/A",
                IsPotential = rawItem.IsPotential,
                ClientTitle = rawItem.ClientTitle,
                ClientType = rawItem.ClientType ?? "N/A",
                PropertyType = rawItem.PropertyType ?? "N/A",
                IsVip = rawItem.IsVip,
                BudgetFrom = rawItem.BudgetFrom,
                BudgetTo = rawItem.BudgetTo,
                CallBackDate = rawItem.CallBackDate,
                CallId = rawItem.CallId,
                AlreadyExistCount = rawItem.AlreadyExistCount,
                CurrentAgent = rawItem.CurrentAgent ?? "N/A",
                CurrentAgentId = rawItem.CurrentAgentId,
                ExtendedDate = rawItem.ExtendedDate,
                FormName = rawItem.FormName ?? "N/A",
                IsClosed = rawItem.IsClosed,
                LastAssignedDate = rawItem.LastAssignedDate,
                LeadTicketStatus = rawItem.LeadTicketStatus ?? "N/A",
                LeadTicketStatusId = (LeadTicketStatuses)rawItem.LeadTicketStatusId,
                LeadTicketNote = rawItem.LeadTicketNote ?? "N/A",
                IsFullLeadTicket = rawItem.IsFullLeadTicket,
                IsVoided = rawItem.IsVoided,
                CorporateCompany = rawItem.CorporateCompany ?? "N/A",
                Currency = rawItem.Currency ?? "N/A",
                DirectManagerName = rawItem.DirectManagerName ?? "N/A",
                WasOld = rawItem.WasOld,
                ViewedByCurAgent = rawItem.ViewedByCurAgent,
                UsageId = (LeadTicketUsages)rawItem.UsageId,
                TopMostManagerName = rawItem.TopMostManagerName ?? "N/A",
                CurrencySymbol = rawItem.CurrencySymbol,
                TcrTypeId = rawItem.TcrTypeId,
                TcrStatusId = rawItem.TcrStatusId,
                ServiceId = (LeadTicketServices)rawItem.ServiceId,
                ServiceFontColor = rawItem.ServiceFontColor,
                ServiceBackgroundColor = rawItem.ServiceBackgroundColor,
                Service = rawItem.Service ?? "N/A",
                SalesTypeId = (SalesTypes)rawItem.SalesTypeId,
                SalesTypeFontColor = rawItem.SalesTypeFontColor,
                SalesTypeBackgroundColor = rawItem.SalesTypeBackgroundColor,
                ReferralId = rawItem.ReferralId,
                StatusFontColor = rawItem.StatusFontColor,
                StatusBackgroundColor = rawItem.StatusBackgroundColor,
                ReassignCount = rawItem.ReassignCount,
                ReassignedNewOnce = rawItem.ReassignedNewOnce,
                ReassignedOnce = rawItem.ReassignedOnce,
                PendingAlreadyExistView = rawItem.PendingAlreadyExistView,
                MarketingAgency = rawItem.MarketingAgency ?? "N/A",
                UsageBackgroundColor = rawItem.UsageBackgroundColor,
                UsageFontColor = rawItem.UsageFontColor,
                SettingInProgressCount = rawItem.SettingInProgressCount,
                ClientCategory = rawItem.ClientCategory ?? "N/A",
                ClientBirthDate = rawItem.ClientBirthDate,
                ClientCompanyName = rawItem.ClientCompanyName ?? "N/A",
                ClientOccupation = rawItem.ClientOccupation ?? "N/A",
                WorkField = rawItem.WorkField ?? "N/A",
                OriginalProject = rawItem.OriginalProject ?? "N/A",
                Gender = rawItem.Gender ?? "N/A",
                AgentLastEventComment = rawItem.AgentLastEventComment,
                AgentLastEventContactingType =
                    rawItem.AgentLastEventContactingTypeId == 0
                        ? "N/A"
                        : ((ContactingTypes)rawItem.AgentLastEventContactingTypeId).ToString(),
                AgentLastEventId = rawItem.AgentLastEventId,
                LastEventComment = rawItem.LastEventComment,
                CallLaterCount = rawItem.CallLaterCount,
                UnQualifiedDistinctCount = rawItem.UnQualifiedDistinctCount,
                UnQualifiedCount = rawItem.UnQualifiedCount,
                QualifiedDistinctCount = rawItem.QualifiedDistinctCount,
                QualifiedCount = rawItem.QualifiedCount,
                NoAnswerDistinctCount = rawItem.NoAnswerDistinctCount,
                CallLaterDistinctCount = rawItem.CallLaterDistinctCount,
                HoursToGetInProgress = rawItem.HoursToGetInProgress,
                LastEventCreationDateTime = rawItem.LastEventCreationDateTime,
                LastEventId = rawItem.LastEventId,
                NoAnswerCount = rawItem.NoAnswerCount,
                AgentLastActivityDate = rawItem.AgentLastEventCreationDateTime,
                BranchId = rawItem.BranchId,
                CompanyId = rawItem.CompanyId,
                LastEventProcess =
                    rawItem.LastEventProcessId == 0
                        ? "N/A"
                        : ((EventProcesses)rawItem.LastEventProcessId).ToString(),
                SetInProgressDate = rawItem.SetInProgressDate,
                ActiveCallLaterCount = rawItem.ActiveCallLaterCount,
                ActiveRemindersCount = rawItem.ActiveRemindersCount,
                RemindersCount = rawItem.RemindersCount,
                RemindersDistinctCount = rawItem.RemindersDistinctCount,
                ResignDate = rawItem.ResignDate,
                IsApproved = rawItem.IsApproved,
                LastAgentFeedBack = rawItem.LastAgentFeedBack,
                LastAgentFeedBackId = rawItem.LastAgentFeedBackId,
                LastAgentFeedBackNote = rawItem.LastAgentFeedBackNote,
                ManagersIdsArray = rawItem.ManagersIdsArray,
            };

            if (rawItem.ContactDevicesJsonb != null)
            {
                var devices =
                    UtilityFunctions.DeserializeJsonDocument<List<ClientPhoneItem>>(rawItem.ContactDevicesJsonb);

                item.ClientContactDevices = devices;

                item.ClientPhone = string.Join(", ", devices.OrderBy(z => z.DeviceType)
                    .Select(z => z.DeviceInfo)
                    .ToArray());
            }

            if (rawItem.RecentEventsJsonb != null)
            {
                var events = UtilityFunctions.DeserializeJsonDocument<List<SystemEventItem>>(rawItem.RecentEventsJsonb);
                item.SystemEvents = events;
            }

            if (rawItem.ActivityStatsJsonb != null)
            {
                var activityStats =
                    UtilityFunctions.DeserializeJsonDocument<ActivityStatItem[]>(rawItem.ActivityStatsJsonb);

                item.ActivityStats = activityStats;
            }

            items.Add(item);
        }

        retObj.Items = items.ToArray();

        return retObj;
    }

    public async Task<List<LeadTicket>> GetLeadTicketEntitiesAsync(List<long> itemsIds, bool fullItemsOnly,
        bool includeExtendedEntity)
    {
        var query = appDb.LeadTickets.Where(z => itemsIds.Contains(z.Id))
            .AsQueryable();

        if (fullItemsOnly)
        {
            query = query.Where(z => z.IsFullLeadTicket && z.IsOld == false);
        }

        if (includeExtendedEntity)
        {
            query = query.Include(z => z.LeadTicketExtension);
        }

        return await query.ToListAsync();
    }

    public async Task UpdateEntitiesAsync(IEnumerable<LeadTicket> leadTickets)
    {
        appDb.LeadTickets.UpdateRange(leadTickets);
        await appDb.SaveBulkChangesAsync();
    }

    public async Task<bool> CheckTcrExistsAsync(long id)
    {
        var deletedStatus = (int)PrimeTcrStatuses.Deleted;

        return await appDb.PrimeTcrs.AnyAsync(z =>
            z.LeadTicketId == id && z.IsDeleted != true && z.PrimeTcrStatusId != deletedStatus);
    }

    public async Task<List<LeadTicketInfoForEmail>> GetLeadTicketForEmailsAsync(long[] affectedIds)
    {
        var items = await appDb.VwLeadTickets.AsNoTracking()
            .Where(z => affectedIds.Contains(z.Id))
            .Select(x => new LeadTicketInfoForEmail
            {
                AgentId = x.CurrentAgentId,
                ClientName = x.ClientName,
                PropertyTypeId = x.PropertyTypeId,
                ServiceId = x.ServiceId,
                ContactDevicesJsonb = x.ContactDevicesJsonb,
            }).ToListAsync();

        foreach (var item in items)
        {
            if (item.ContactDevicesJsonb != null)
            {
                var devices =
                    UtilityFunctions.DeserializeJsonDocument<List<ClientPhoneItem>>(item.ContactDevicesJsonb);

                var device = devices.OrderBy(z => z.IsDefault).FirstOrDefault(z => z.DeviceTypeId != 4);

                if (device != null)
                {
                    item.ClientPhone = device.DeviceInfo;
                }
            }
        }
        return items;

    }

    private async Task<IOrderedQueryable<VwLeadTicket>> PopulateSorting(IQueryable<VwLeadTicket> rawLeads,
        LeadTicketsFiltersModel filters, ILookUpsService lookUpsService)
    {
        if (string.IsNullOrEmpty(filters.SortField))
        {
            return rawLeads.OrderBy("Id desc");
        }

        if (filters.SortField == "AgentLastActivityDate")
        {
            return rawLeads.OrderBy($"AgentLastEventCreationDateTimeNumeric {filters.SortDirection}");
        }

        if (filters.SortField == "CreationDate")
        {
            return rawLeads.OrderBy($"CreationDateNumeric {filters.SortDirection}");
        }

        if (filters.SortField == "LastAssignedDate")
        {
            return rawLeads.OrderBy($"LastAssignedDateNumeric {filters.SortDirection}");

            ;
        }

        var sortedList = new List<int>();

        if (filters.SortField == "LeadSource")
        {
            var lookups = await lookUpsService.GetLeadSourcesAsync();

            sortedList = lookups.AsQueryable()
                .OrderBy($"ItemName {filters.SortDirection}")
                .Select(z => z.ItemId)
                .ToList();

            Expression<Func<VwLeadTicket, int>> keySelector = lt => lt.LeadSourceId;

            return ApplySorting(rawLeads, sortedList, keySelector);
        }

        if (filters.SortField == "PropertyType")
        {
            var lookups = await lookUpsService.GetPropertyTypesAsync(filters.UsageId);

            sortedList = lookups.AsQueryable()
                .OrderBy($"ItemName {filters.SortDirection}")
                .Select(z => z.ItemId)
                .ToList();

            Expression<Func<VwLeadTicket, int>> keySelector = lt => lt.PropertyTypeId;

            return ApplySorting(rawLeads, sortedList, keySelector);
        }

        if (filters.SortField == "SalesType")
        {
            var lookups = await lookUpsService.GetSalesTypesAsync();

            sortedList = lookups.AsQueryable()
                .OrderBy($"ItemName {filters.SortDirection}")
                .Select(z => z.ItemId)
                .ToList();

            Expression<Func<VwLeadTicket, int>> keySelector = lt => lt.SalesTypeId;

            return ApplySorting(rawLeads, sortedList, keySelector);
        }

        if (filters.SortField == "Service")
        {
            var lookups = await lookUpsService.GetServicesAsync();

            sortedList = lookups.AsQueryable()
                .OrderBy($"ItemName {filters.SortDirection}")
                .Select(z => z.ItemId)
                .ToList();

            Expression<Func<VwLeadTicket, int>> keySelector = lt => lt.ServiceId;

            return ApplySorting(rawLeads, sortedList, keySelector);
        }

        if (filters.SortField == "Usage")
        {
            var lookups = await lookUpsService.GetUsageAsync();

            sortedList = lookups.AsQueryable()
                .OrderBy($"ItemName {filters.SortDirection}")
                .Select(z => z.ItemId)
                .ToList();

            Expression<Func<VwLeadTicket, int>> keySelector = lt => lt.UsageId;

            return ApplySorting(rawLeads, sortedList, keySelector);
        }

        return rawLeads.OrderBy($"{filters.SortField} {filters.SortDirection}");
    }

    private IOrderedQueryable<VwLeadTicket> ApplySorting(IQueryable<VwLeadTicket> query, List<int> sortedList,
        Expression<Func<VwLeadTicket, int>> keySelector)
    {
        var param = keySelector.Parameters.First();
        Expression? orderExpression = null;

        for (var i = 0; i < sortedList.Count; i++)
        {
            var condition = Expression.Equal(keySelector.Body, Expression.Constant(sortedList[i]));
            var value = Expression.Constant(i + 1); // Rank starts from 1

            orderExpression = orderExpression == null
                ? Expression.Condition(condition, value, Expression.Constant(sortedList.Count + 1))
                : Expression.Condition(condition, value, orderExpression);
        }

        var lambda = Expression.Lambda<Func<VwLeadTicket, int>>(orderExpression!, param);
        var orderedQuery = query.OrderBy(lambda);

        return orderedQuery;
    }
}
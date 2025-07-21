using CloudBlue.Domain.DataModels.PrimeTcrs;
using CloudBlue.Domain.DomainModels;
using CloudBlue.Domain.DomainModels.Filtration.Interfaces;
using CloudBlue.Domain.DomainModels.PrimeTcrs;
using CloudBlue.Domain.Enums;
using CloudBlue.Domain.GenericTypes;
using CloudBlue.Domain.Interfaces.DbContext;
using CloudBlue.Domain.Interfaces.Repositories;
using CloudBlue.Domain.Interfaces.Services;
using CloudBlue.Domain.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CloudBlue.Data.Repositories;
public class ParameterReplacer : ExpressionVisitor
{
    private readonly ParameterExpression _oldParameter;
    private readonly ParameterExpression _newParameter;

    public ParameterReplacer(ParameterExpression oldParameter, ParameterExpression newParameter)
    {
        _oldParameter = oldParameter;
        _newParameter = newParameter;
    }

    protected override Expression VisitParameter(ParameterExpression node)
    {
        return node == _oldParameter ? _newParameter : base.VisitParameter(node);
    }
}

public class PrimeTcrsRepository(ICrmDataContext dbContext, ILookUpsService lookUpsService) : IPrimeTcrsRepository
{
    public int CurrentUserId { get; set; }
    public int CurrentUserBranchId { get; set; }
    public int CurrentUserCompanyId { get; set; }
    public long LastCreatedItemId { get; set; }

    public async Task<bool> CreatePrimeTcrAsync(CreatePrimeTcrFullModel model)
    {
        var tcr = new PrimeTcr
        {
            BranchId = model.BranchId,
            BuildUpArea = model.BuildUpArea,
            ClientId = model.ClientId,
            ClosingChannelExtraId = model.ClosingChannelExtraId,
            ClosingChannelId = model.ClosingChannelId,
            CollectedCommissionValue = 0,
            CompanyCommissionPercentage = model.CompanyCommissionPercentage,
            CompanyCommissionValue = model.CompanyCommissionValue,
            CompanyId = model.CompanyId,
            ConfirmedContractingDate = null,
            ConfirmedContractingDateNumeric = 0,
            ConfirmedHalfContractingDate = null,
            ConfirmedHalfContractingDateNumeric = 0,
            ConfirmedReservingDate = null,
            ConfirmedReservingDateNumeric = 0,
            ContractExpectedDate = model.ContractExpectedDate,
            CreatedBy = model.CreatedBy,
            CreatedById = model.CreatedById,
            CreationDateNumeric = model.CreationDateNumeric,
            CreationDateTime = model.CreationDateTime,
            CutOffId = 0,
            DeveloperId = model.DeveloperId,
            DeveloperName = model.DeveloperName,
            DocumentDate = model.DocumentDate,
            DocumentTypeId = model.DocumentTypeId,
            DueBalance = model.DueBalance,
            ExtraManagerId = model.ExtraManagerId,
            ExtraManagerName = model.ExtraManagerName,
            FirstAgentForcedCommissioningPercentage = model.FirstAgentForcedCommissioningPercentage,
            FirstAgentId = model.FirstAgentId,
            FirstAgentInResaleTeam = model.FirstAgentInResaleTeam,
            FirstAgentName = model.FirstAgentName,
            FirstAgentPositionAtContractDateId = 0,
            FirstAgentPositionAtHalfContractDateId = 0,
            FirstAgentPositionId = model.FirstAgentPositionId,
            FirstAgentPromoted = false,
            FirstAgentSharePercentage = model.FirstAgentSharePercentage,
            FirstAgentTreeJsonb = model.FirstAgentTreeJsonb,
            FirstAgentTreeNames = model.FirstAgentTreeNames,
            FirstReferralAgentId = 0,
            FirstReferralAgentInResaleTeam = false,
            FirstReferralAgentName = null,
            FirstReferralAgentPositionId = 0,
            FirstReferralAgentPromoted = false,
            FirstReferralAgentTreeJsonb = null,
            FirstReferralAgentTreeNames = null,
            FirstReferralItemId = 0,
            ForceAchievementPercentage = model.ForceAchievementPercentage,
            ForceCommissionPercentage = model.ForceCommissionPercentage,
            ForcedAgentIncentiveValue = 0,
            ForcedScaledCommissionPercentage = model.ForcedScaledCommissionPercentage,
            ForceFlatRateCommission = model.ForceFlatRateCommission,
            ForceHalfDeal = model.ForceHalfDeal,
            FreezeCommissionAgentsIds = null,
            FreezeCommissionManagersIds = null,
            HalfCommissionCutOffId = 0,
            HalfCommissionPaid = false,
            HaveDocument = model.HaveDocument,
            IgnoreDebitedCommission = false,
            IgnoreDebitedIncentive = false,
            Invoiced = false,
            IsCompanyCommissionCollected = false,
            IsCorporate = model.IsCorporate,
            IsDeleted = false,
            IsHalfCommission = model.IsHalfCommission,
            IsHalfContracted = model.IsHalfContracted,
            IsRegular = model.IsRegular,
            IsReOpen = false,
            IsResolved = false,
            LandArea = model.LandArea,
            LastConflictDate = null,
            LastConflictDateNumeric = 0,
            LastDeveloperFeedBack = null,
            LastDeveloperFeedBackDate = null,
            LastDeveloperFeedBackDateNumeric = 0,
            LastDeveloperReviewingDate = null,
            LastDeveloperReviewingDateNumeric = 0,
            LastFeedBack = null,
            LastMarketingChannelExtraId = model.LastMarketingChannelExtraId,
            LastMarketingChannelId = model.LastMarketingChannelId,
            LastPostponeDate = null,
            LastPostponeDateNumeric = 0,
            LastReopenDate = null,
            LastReopenDateNumeric = 0,
            LastResolveDate = null,
            LastResolveDateNumeric = 0,
            LeadTicketId = model.LeadTicketId,
            OutsideBrokerId = model.OutsideBrokerId,
            OutsideBrokerName = model.OutsideBrokerName,
            PendingStageId = model.PendingStageId,
            Phase = model.Phase,
            PrimeTcrStatusId = model.PrimeTcrStatusId,
            ProjectId = model.ProjectId,
            ProjectName = model.ProjectName,
            PropertyTypeId = model.PropertyTypeId,
            RecCloseDate = model.RecCloseDate,
            RecCloseDateNumeric = model.RecCloseDateNumeric,
            RecReserveDate = model.RecReserveDate,
            RecReserveDateNumeric = model.RecReserveDateNumeric,
            Remarks = model.Remarks,
            ResignedRuleSkippedIds = null,
            RestrictTargetCommission = false,
            SalesAccountingFeedBackId = 0,
            SalesVolume = model.SalesVolume,
            SecondAgentForcedCommissioningPercentage = model.SecondAgentForcedCommissioningPercentage,
            SecondAgentId = model.SecondAgentId,
            SecondAgentInResaleTeam = model.SecondAgentInResaleTeam,
            SecondAgentName = model.SecondAgentName,
            SecondAgentPositionAtContractDateId = 0,
            SecondAgentPositionAtHalfContractDateId = 0,
            SecondAgentPositionId = model.SecondAgentPositionId,
            SecondAgentPromoted = false,
            SecondAgentSharePercentage = model.SecondAgentSharePercentage,
            SecondAgentTreeJsonb = model.SecondAgentTreeJsonb,
            SecondAgentTreeNames = model.SecondAgentTreeNames,
            SecondReferralAgentId = 0,
            SecondReferralAgentInResaleTeam = false,
            SecondReferralAgentName = null,
            SecondReferralAgentPositionId = 0,
            SecondReferralAgentPromoted = false,
            SecondReferralAgentTreeJsonb = null,
            SecondReferralAgentTreeNames = null,
            SecondReferralItemId = 0,
            SkipIncentive = false,
            SkipHalfCommissionRules = false,
            TaxPercentage = model.TaxPercentage,
            TcrSelection = false,
            ThirdAgentForcedCommissioningPercentage = model.ThirdAgentForcedCommissioningPercentage,
            ThirdAgentId = model.ThirdAgentId,
            ThirdAgentInResaleTeam = model.ThirdAgentInResaleTeam,
            ThirdAgentName = model.ThirdAgentName,
            ThirdAgentPositionAtContractDateId = 0,
            ThirdAgentPositionAtHalfContractDateId = 0,
            ThirdAgentPositionId = model.ThirdAgentPositionId,
            ThirdAgentPromoted = false,
            ThirdAgentSharePercentage = model.ThirdAgentSharePercentage,
            ThirdAgentTreeJsonb = model.ThirdAgentTreeJsonb,
            ThirdAgentTreeNames = model.ThirdAgentTreeNames,
            UnitNumber = model.UnitNumber,
            ClientName = model.ClientName,
            UnitNumberLowered = model.UnitNumber.ToLower(),
            VerificationComment = null,
            VerificationStatusId = 0,
            UsageId = model.UsageId,
            OutsideBrokerCommissionPercentage = model.OutsideBrokerCommissionPercentage,
            OutsideBrokerCommissionValue = model.OutsideBrokerCommissionValue,
            TeleSalesAgentName = model.TeleSalesAgentName,
            TeleSalesAgentId = model.TeleSalesAgentId,
            AgentsIdsArray = model.AgentsIdsArray,
            ManagersIdsArray = model.ManagersIdsArray,
        };

        dbContext.PrimeTcrs.Add(tcr);
        await dbContext.SaveChangesAsync();
        LastCreatedItemId = tcr.Id;

        return true;
    }

    private IEnumerable<LookupItem<int>> _postions = [];
    public async Task<ListResult<PrimeTcrItemForList>> GetPrimeTcrsAsync(PrimeTcrsFiltersModel filters)
    {
        var rawQuery = dbContext.VwPrimeTcrs.AsQueryable();
        rawQuery = SetNumericFilters(rawQuery, filters);
        rawQuery = SetSimpleDateFilters(rawQuery, filters);
        rawQuery = SetIdsFilters(rawQuery, filters);
        rawQuery = SetSpecialFilters(rawQuery, filters);
        rawQuery = SetConfirmationDateFilters(rawQuery, filters);
        rawQuery = SetAgentsFilters(rawQuery, filters);
        rawQuery = SetComplexDateFilters(rawQuery, filters);
        var retObj = new ListResult<PrimeTcrItemForList>();
        retObj.TotalCount = await rawQuery.CountAsync();
        rawQuery = rawQuery.OrderByDescending(z => z.CreationDateNumeric);

        var rawItems = await rawQuery.Skip(filters.PageIndex * filters.PageSize)
            .Take(filters.PageSize)
            .AsNoTracking()
                  .ToArrayAsync();

        var items = new PrimeTcrItemForList[rawItems.Length];
        _postions = await lookUpsService.GetUserPositionsAsync();
        for (var i = 0; i < rawItems.Length; i++)
        {
            var rawItem = rawItems[i];

            var item = new PrimeTcrItemForList
            {
                Id = rawItem.Id,
                UnitNumber = rawItem.UnitNumber,
                Usage = rawItem.Usage,
                PrimeTcrStatusBackgroundColor = rawItem.PrimeTcrStatusBackgroundColor,
                LeadSource = rawItem.LeadSource,
                SalesVolume = rawItem.SalesVolume,
                ContractExpectedDate = rawItem.ContractExpectedDate,
                OutsideBrokerName = rawItem.OutsideBrokerName,
                ExtraManagerName = rawItem.ExtraManagerName,
                MarketingAgency = rawItem.Agency,
                IsCorporate = rawItem.IsCorporate,
                CreatedBy = rawItem.CreatedBy,
                CreationDateTime = rawItem.CreationDateTime,
                LeadTicketId = rawItem.LeadTicketId,
                FirstAgentName = rawItem.FirstAgentName,
                IsReOpen = rawItem.IsReOpen,
                RecReserveDate = rawItem.RecReserveDate,
                RecCloseDate = rawItem.RecCloseDate,
                LastFeedBack = rawItem.LastFeedBack,
                LastPostponeDate = rawItem.LastPostponeDate,
                IsResolved = rawItem.IsResolved,
                ConfirmedContractingDate = rawItem.ConfirmedContractingDate,
                IsHalfContracted = rawItem.IsHalfContracted,
                LastDeveloperReviewingDate = rawItem.LastDeveloperReviewingDate,
                LastDeveloperFeedBack = rawItem.LastDeveloperFeedBack,
                LastDeveloperFeedBackDate = rawItem.LastDeveloperFeedBackDate,
                LastResolveDate = rawItem.LastResolveDate,
                LastConflictDate = rawItem.LastConflictDate,
                Invoiced = rawItem.Invoiced,
                LastReopenDate = rawItem.LastReopenDate,
                ClientName = rawItem.ClientName,
                PropertyType = rawItem.PropertyType,
                ProjectName = rawItem.ProjectName,
                DeveloperName = rawItem.DeveloperName,
                PrimeTcrStatusName = rawItem.PrimeTcrStatusName,
                LeadTicketCreationDate = rawItem.LeadTicketCreationDate,
                ClosingChannel = rawItem.ClosingChannel,
                ClosingSubChannel = rawItem.ClosingSubChannel,
                LastMarketingChannel = rawItem.LastMarketingChannel,
                LastMarketingSubChannel = rawItem.LastMarketingSubChannel,
                PrimeTcrStatusId = rawItem.PrimeTcrStatusId,
                VerificationStatusId = rawItem.VerificationStatusId,
                IsCompanyCommissionCollected = rawItem.IsCompanyCommissionCollected,
                DueBalance = rawItem.DueBalance,
                HalfCommissionPaid = rawItem.HalfCommissionPaid,
                IsHalfCommission = rawItem.IsHalfCommission,
                DocumentDate = rawItem.DocumentDate,
                DocumentTypeId = rawItem.DocumentTypeId,
                HaveDocument = rawItem.HaveDocument,
                BranchName = filters.UseTcrCompany ? rawItem.TcrBranchName : rawItem.LeadTicketBranchName,
                CompanyName = filters.UseTcrCompany ? rawItem.TcrCompanyName : rawItem.LeadTicketCompanyName,
                BranchId = filters.UseTcrCompany ? rawItem.TcrBranchId : rawItem.LeadTicketBranchId,
                CompanyId = filters.UseTcrCompany ? rawItem.TcrCompanyId : rawItem.LeadTicketCompanyId,
                AgentsIdsArray = rawItem.AgentsIdsArray,
                ManagersIdsArray = rawItem.ManagersIdsArray,
                ForceHalfDeal = rawItem.ForceHalfDeal,
                ConfirmedHalfContractingDate = rawItem.ConfirmedHalfContractingDate,
                ConfirmedReservingDate = rawItem.ConfirmedReservingDate,
                BuildUpArea = rawItem.BuildUpArea,
                ConvertedFromReferral = rawItem.ReferralId > 0,
                FirstAgent = rawItem.FirstAgentName,
                FirstAgentId = rawItem.FirstAgentId,
                DocumentType = rawItem.DocumentTypeId == 0 ? "N/A" : rawItem.DocumentTypeId == 1 ? "Reservation" : "Contract",
                FirstAgentManagerOne = "N/A",
                FirstAgentManagerTwo = "N/A",
                FirstAgentManagerThree = "N/A",
                FirstAgentManagerFour = "N/A",
                FirstAgentManagerFive = "N/A",

                ThirdAgentManagerOne = "N/A",
                ThirdAgentManagerTwo = "N/A",
                ThirdAgentManagerThree = "N/A",
                ThirdAgentManagerFour = "N/A",
                ThirdAgentManagerFive = "N/A",
                SecondAgentManagerOne = "N/A",
                SecondAgentManagerTwo = "N/A",
                SecondAgentManagerThree = "N/A",
                SecondAgentManagerFour = "N/A",
                SecondAgentManagerFive = "N/A",
                FirstAgentPercentage = rawItem.FirstAgentSharePercentage,
                SecondAgentId = rawItem.SecondAgentId,
                FirstAgentPosition = GetAgentPosition(rawItem.FirstAgentPositionId),
                ThirdAgentPosition = GetAgentPosition(rawItem.ThirdAgentPositionId),
                SecondAgentPosition = GetAgentPosition(rawItem.SecondAgentPositionId),
                ThirdAgentPercentage = rawItem.ThirdAgentSharePercentage,
                ThirdAgentId = rawItem.ThirdAgentId,
                ThirdAgent = rawItem.ThirdAgentName,
                SecondAgent = rawItem.SecondAgentName,
                SecondAgentPercentage = rawItem.SecondAgentSharePercentage,
                LeadProject = rawItem.LeadProject,
                LeadDistrict = rawItem.LeadDistrict,
                LastReopenReason = rawItem.LastReOpenReason,
                FirstAgentTreeNames = rawItem.FirstAgentTreeNames,
                ThirdAgentTreeNames = rawItem.ThirdAgentTreeNames,
                SecondAgentTreeNames = rawItem.SecondAgentTreeNames,



            };

            if (string.IsNullOrEmpty(item.FirstAgentTreeNames) == false)
            {
                var arr = item.FirstAgentTreeNames.Split(',');

                if (arr.Length == 1)
                {
                    item.FirstAgentManagerOne = arr[0];
                }
                else if (arr.Length == 2)
                {
                    item.FirstAgentManagerOne = arr[1];
                    item.FirstAgentManagerTwo = arr[0];
                }
                if (arr.Length == 3)
                {
                    item.FirstAgentManagerOne = arr[2];
                    item.FirstAgentManagerTwo = arr[1];
                    item.FirstAgentManagerThree = arr[0];
                }
                else if (arr.Length == 4)
                {
                    item.FirstAgentManagerOne = arr[3];
                    item.FirstAgentManagerTwo = arr[2];
                    item.FirstAgentManagerThree = arr[1];
                    item.FirstAgentManagerFour = arr[0];

                }
                else if (arr.Length == 5)
                {
                    item.FirstAgentManagerOne = arr[4];
                    item.FirstAgentManagerTwo = arr[3];
                    item.FirstAgentManagerThree = arr[2];
                    item.FirstAgentManagerFour = arr[1];
                    item.FirstAgentManagerFive = arr[0];
                }

            }

            if (string.IsNullOrEmpty(item.SecondAgentTreeNames) == false)
            {
                var arr = item.SecondAgentTreeNames.Split(',');

                if (arr.Length == 1)
                {
                    item.SecondAgentManagerOne = arr[0];
                }
                else if (arr.Length == 2)
                {
                    item.SecondAgentManagerOne = arr[1];
                    item.SecondAgentManagerTwo = arr[0];
                }
                if (arr.Length == 3)
                {
                    item.SecondAgentManagerOne = arr[2];
                    item.SecondAgentManagerTwo = arr[1];
                    item.SecondAgentManagerThree = arr[0];
                }
                else if (arr.Length == 4)
                {
                    item.SecondAgentManagerOne = arr[3];
                    item.SecondAgentManagerTwo = arr[2];
                    item.SecondAgentManagerThree = arr[1];
                    item.SecondAgentManagerFour = arr[0];

                }
                else if (arr.Length == 5)
                {
                    item.SecondAgentManagerOne = arr[4];
                    item.SecondAgentManagerTwo = arr[3];
                    item.SecondAgentManagerThree = arr[2];
                    item.SecondAgentManagerFour = arr[1];
                    item.SecondAgentManagerFive = arr[0];
                }

            }


            if (string.IsNullOrEmpty(item.ThirdAgentTreeNames) == false)
            {
                var arr = item.ThirdAgentTreeNames.Split(',');

                if (arr.Length == 1)
                {
                    item.ThirdAgentManagerOne = arr[0];
                }
                else if (arr.Length == 2)
                {
                    item.ThirdAgentManagerOne = arr[1];
                    item.ThirdAgentManagerTwo = arr[0];
                }
                if (arr.Length == 3)
                {
                    item.ThirdAgentManagerOne = arr[2];
                    item.ThirdAgentManagerTwo = arr[1];
                    item.ThirdAgentManagerThree = arr[0];
                }
                else if (arr.Length == 4)
                {
                    item.ThirdAgentManagerOne = arr[3];
                    item.ThirdAgentManagerTwo = arr[2];
                    item.ThirdAgentManagerThree = arr[1];
                    item.ThirdAgentManagerFour = arr[0];

                }
                else if (arr.Length == 5)
                {
                    item.ThirdAgentManagerOne = arr[4];
                    item.ThirdAgentManagerTwo = arr[3];
                    item.ThirdAgentManagerThree = arr[2];
                    item.ThirdAgentManagerFour = arr[1];
                    item.ThirdAgentManagerFive = arr[0];
                }

            }



            if (rawItem.RecentEventsJsonb != null && filters.ExportMode == false)
            {
                var events = UtilityFunctions.DeserializeJsonDocument<List<SystemEventItem>>(rawItem.RecentEventsJsonb);
                item.SystemEvents = events;
            }

            items[i] = item;
        }

        retObj.Items = items;

        return retObj;
    }

    private string? GetAgentPosition(int positionId)
    {
        var position = _postions.FirstOrDefault(z => z.ItemId == positionId);

        if (position == null)
        {
            return "N/A";
        }

        return position.ItemName;
    }

    public async Task<long> CheckUnitNumberExistAsync(string unitNumber, int projectId, int[] statusesToExclude)
    {
        return await dbContext.PrimeTcrs.Where(z =>
            z.UnitNumberLowered == unitNumber.ToLower() && z.ProjectId == projectId && z.IsDeleted != false &&
            statusesToExclude.Contains(z.PrimeTcrStatusId) == false).Select(z => z.Id).FirstOrDefaultAsync();
    }

    public async Task<PrimeTcrFullItem?> GetSinglePrimeTcrAsync(PrimeTcrItemFiltersModel filter)
    {
        var query = dbContext.VwPrimeTcrs.AsNoTracking().Where(z => z.Id == filter.PrimeTcrId)
            .AsQueryable();

        if (filter.CompanyId > 0)
        {
            if (filter.UseTcrCompany)
            {
                query = query.Where(z => z.TcrCompanyId == filter.CompanyId);
            }
            else
            {
                query = query.Where(z => z.LeadTicketCompanyId == filter.CompanyId);
            }
        }
        if (filter.BranchId > 0)
        {
            if (filter.UseTcrCompany)
            {
                query = query.Where(z => z.TcrBranchId == filter.BranchId);
            }
            else
            {
                query = query.Where(z => z.LeadTicketBranchId == filter.BranchId);
            }
        }


        if (filter.AgentId > 0 || filter.ManagersIds.Any())
        {
            query = SetAgentsFiltersForSingleTcr(query, filter);
        }



        var tcr = await query.FirstOrDefaultAsync();

        if (tcr == null)
        {
            return null;
        }

        var returnTcr = MapObj(tcr, filter);
        if (returnTcr == null)
        {
            return null;
        }
        returnTcr.PrimeTcrAttachments = await dbContext.PrimeTcrAttachments.Where(z => z.PrimeTcrId == tcr.Id).Select(z => new PrimeTcrAttachmentItem { AttachmentDate = z.AttachmentDate, Id = z.Id, OriginalFileName = z.OriginalFileName, TcrAttachmentDescription = z.TcrAttachmentDescription, UploadedBy = z.UploadedBy, PrimeTcrId = z.PrimeTcrId }).ToArrayAsync();

        return returnTcr;
    }

    public async Task<PrimeTcr[]> GetPrimeTcrsEntitiesAsync(List<long> modelItemsIds)
    {
        return await dbContext.PrimeTcrs.Where(z => z.IsDeleted == false && modelItemsIds.Contains(z.Id)).ToArrayAsync();
    }

    public async Task UpdateEntitiesAsync(List<PrimeTcr> affectedPrimeTcrs)
    {


        await dbContext.PrimeTcrs.BulkUpdateAsync(affectedPrimeTcrs);
        await dbContext.SaveBulkChangesAsync();


    }

    public async Task SaveAttachmentAsync(PrimeTcrAttachment attachment)
    {
        dbContext.PrimeTcrAttachments.Add(attachment);
        await dbContext.SaveChangesAsync();
    }

    private IQueryable<VwPrimeTcr> SetAgentsFiltersForSingleTcr(IQueryable<VwPrimeTcr> rawQuery, PrimeTcrItemFiltersModel filters)
    {

        if (filters.AgentId == 0 && filters.ManagersIds.Any() == false)
        {
            return rawQuery;
        }
        if (filters.AgentId > 0 && filters.ManagersIds.Any() == false)
        {
            rawQuery = rawQuery.Where(z => z.AgentsIdsArray.Contains(filters.AgentId));

            return rawQuery;
        }

        if (filters.AgentId == 0 && filters.ManagersIds.Any())
        {
            rawQuery = rawQuery.Where(z => z.ManagersIdsArray.Any(r => filters.ManagersIds.Contains(r)));

            return rawQuery;
        }


        if (filters.UserAgentOr)
        {
            rawQuery = rawQuery.Where(z => (z.ManagersIdsArray.Any(r => filters.ManagersIds.Contains(r)) || z.AgentsIdsArray.Contains(filters.AgentId)));
        }
        else
        {
            rawQuery = rawQuery.Where(z => (z.ManagersIdsArray.Any(r => filters.ManagersIds.Contains(r)) && z.AgentsIdsArray.Contains(filters.AgentId)));
        }

        return rawQuery;

    }

    private PrimeTcrFullItem? MapObj(VwPrimeTcr tcr, PrimeTcrItemFiltersModel filter)
    {
        var item = new PrimeTcrFullItem
        {
            BuildUpArea = tcr.BuildUpArea,
            ClientName = tcr.ClientName,
            ConfirmedContractingDate = tcr.ConfirmedContractingDate,
            ConfirmedHalfContractingDate = tcr.ConfirmedHalfContractingDate,
            ClientPhones = "N/A",
            ContractExpectedDate = tcr.ContractExpectedDate,
            CreationDateTime = tcr.CreationDateTime,
            DeveloperName = tcr.DeveloperName,
            DocumentDate = tcr.DocumentDate,
            DocumentType = "N/A",
            ExtraManagerName = tcr.ExtraManagerName,
            FirstAgentName = tcr.FirstAgentName,
            FirstAgentSharePercentage = tcr.FirstAgentSharePercentage * 100m,
            FirstAgentTreeNames = tcr.FirstAgentTreeNames,
            FirstReferralAgentName = tcr.FirstReferralAgentName,
            FirstReferralAgentTreeNames = tcr.FirstReferralAgentTreeNames,
            HalfCommission = "N/A",
            HalfConfirmed = "N/A",
            HasDocument = tcr.HaveDocument,
            Invoiced = tcr.Invoiced,
            InvoicedStr = tcr.Invoiced ? "Yes" : "No",
            IsCorporate = tcr.IsCorporate,
            IsDeleted = tcr.IsDeleted,
            IsReOpen = tcr.IsReOpen,
            IsResolved = tcr.IsResolved,
            LandArea = tcr.LandArea,
            LastConflictDate = tcr.LastConflictDate,
            LastDeveloperFeedBack = tcr.LastDeveloperFeedBack,
            LastDeveloperFeedBackDate = tcr.LastDeveloperFeedBackDate,
            LastDeveloperReviewingDate = tcr.LastDeveloperReviewingDate,
            LastPostponeDate = tcr.LastPostponeDate,
            LastReopenDate = tcr.LastReopenDate,
            LastResolveDate = tcr.LastResolveDate,
            LeadTicketCreationDate =
                tcr.LeadTicketCreationDate,
            LeadTicketId = tcr.LeadTicketId,
            OutsideBrokerName = tcr.OutsideBrokerName,
            PrimeTcrId = tcr.Id,
            PrimeTcrStatusName = tcr.PrimeTcrStatusName,
            ProjectName = tcr.ProjectName,
            PropertyTypeName = tcr.PropertyType,
            RecCloseDate = tcr.RecCloseDate,
            RecReserveDate = tcr.RecReserveDate,
            Remarks = tcr.Remarks,
            SalesVolume = (int)tcr.SalesVolume,
            SecondAgentName = tcr.SecondAgentName,
            SecondAgentSharePercentage = tcr.SecondAgentSharePercentage * 100m,
            SecondAgentTreeNames = tcr.SecondAgentTreeNames,
            SecondReferralAgentName = tcr.SecondReferralAgentName,
            SecondReferralAgentTreeNames = tcr.SecondReferralAgentTreeNames,
            TeleSalesAgentName = tcr.TeleSalesAgentName,
            ThirdAgentName = tcr.ThirdAgentName,
            ThirdAgentSharePercentage = tcr.ThirdAgentSharePercentage * 100m,
            ThirdAgentTreeNames = tcr.ThirdAgentTreeNames,
            UnitNumber = tcr.UnitNumber,
            Usage = tcr.Usage,
            VerificationComment = tcr.VerificationComment,
            VerificationDate = tcr.VerificationDate,
            VerificationStatus = tcr.TcrVerificationStatus,
            ManagersIdsArray = tcr.ManagersIdsArray,
            AgentsIdsArray = tcr.AgentsIdsArray,
            BranchName = filter.UseTcrCompany ? tcr.TcrBranchName : tcr.LeadTicketBranchName,
            CompanyName = filter.UseTcrCompany ? tcr.TcrCompanyName : tcr.LeadTicketCompanyName,

            BranchId = filter.UseTcrCompany ? tcr.TcrBranchId : tcr.LeadTicketBranchId,
            CompanyId = filter.UseTcrCompany ? tcr.TcrCompanyId : tcr.LeadTicketCompanyId,
            PrimeTcrStatusId = tcr.PrimeTcrStatusId,
            HalfCommissionPaid = tcr.HalfCommissionPaid,
            VerificationStatusId = tcr.VerificationStatusId,
            OutsideBrokerCommissionValue = tcr.OutsideBrokerCommissionValue,
            OutsideBrokerCommissionPercentage = tcr.OutsideBrokerCommissionPercentage * 100m,
            CollectedCommissionValue = tcr.CollectedCommissionValue,
            CompanyCommissionValue = tcr.CompanyCommissionValue,
            DueBalance = tcr.DueBalance,

            IsCompanyCommissionCollected = tcr.IsCompanyCommissionCollected,
            IsHalfContracted = tcr.IsHalfContracted,
            FirstAgentId = tcr.FirstAgentId,
            SecondAgentId = tcr.SecondAgentId,
            ThirdAgentId = tcr.ThirdAgentId,
            FirstReferralAgentId = tcr.FirstReferralAgentId,
            SecondReferralAgentId = tcr.SecondReferralAgentId,
            UsageId = tcr.UsageId,
            IsHalfCommission = tcr.IsHalfCommission,
            ForceHalfDeal = tcr.ForceHalfDeal,
            ConfirmedReservingDate = tcr.ConfirmedReservingDate




        };

        if (tcr.DocumentTypeId > 0)
        {
            item.DocumentType = tcr.DocumentTypeId == 1 ? "Reservation" : "Contract";
        }

        if (tcr.IsHalfCommission)
        {
            item.HalfCommission = tcr.HalfCommissionPaid
                ? $"Collected on {tcr.ConfirmedContractingDate?.ToString("dd MMM yyyy")}"
                : "Not Collected Yet";
        }

        if (tcr.IsHalfContracted)
        {
            item.HalfConfirmed = $"50% Confirmed on {tcr.ConfirmedHalfContractingDate?.ToString("dd MMM yyyy")}";
        }

        if (tcr.ContactDevicesJsonb != null)
        {
            var devices = UtilityFunctions.DeserializeJsonDocument<List<ClientPhoneItem>>(tcr.ContactDevicesJsonb);
            item.ClientContactDevices = devices;

            item.ClientPhones = string.Join(", ", devices.OrderBy(z => z.DeviceType)
                .Select(z => z.DeviceInfo)
                .ToArray());
        }

        if (tcr.RecentEventsJsonb != null)
        {
            var events = UtilityFunctions.DeserializeJsonDocument<List<SystemEventItem>>(tcr.RecentEventsJsonb);
            item.SystemEvents = events;
        }

        var tcrConfigs = new TcrConfigsItem
        {
            CompanyCommissionPercentage = tcr.CompanyCommissionPercentage * 100,
            ForceAchievementPercentage = tcr.ForceAchievementPercentage,
            ForceCommissionPercentage = tcr.ForceCommissionPercentage,
            ForceFlatRateCommission = tcr.ForceFlatRateCommission,
            ForceHalfDeal = tcr.ForceHalfDeal,
            ForcedAgentIncentiveValue = tcr.ForcedAgentIncentiveValue,
            ForcedScaledCommissionPercentage = tcr.ForcedScaledCommissionPercentage,
            FreezeCommissionAgentsIdsStr = tcr.FreezeCommissionAgentsIds,
            FreezeCommissionManagersIdsStr = tcr.FreezeCommissionManagersIds,
            IgnoreDebitedCommission = tcr.IgnoreDebitedCommission,
            IgnoreDebitedIncentive = tcr.IgnoreDebitedIncentive,
            IsHalfCommission = tcr.IsHalfCommission,
            IsRegular = tcr.IsRegular,
            ResignedRuleSkippedIdsStr = tcr.ResignedRuleSkippedIds,
            RestrictTargetCommission = tcr.RestrictTargetCommission,
            SkipIncentive = tcr.SkipIncentive,
            SkipHalfCommissionRules = tcr.SkipHalfCommissionRules,
            TaxPercentage = tcr.TaxPercentage * 100m,
            TcrSelection = tcr.TcrSelection,
        };

        if (string.IsNullOrEmpty(tcrConfigs.FreezeCommissionAgentsIdsStr) == false)
        {
            tcrConfigs.FreezeCommissionAgentsIds = UtilityFunctions.GetIntListFromString(tcrConfigs.FreezeCommissionAgentsIdsStr);
        }
        if (string.IsNullOrEmpty(tcrConfigs.FreezeCommissionManagersIdsStr) == false)
        {
            tcrConfigs.FreezeCommissionManagersIds = UtilityFunctions.GetIntListFromString(tcrConfigs.FreezeCommissionManagersIdsStr);
        }
        if (string.IsNullOrEmpty(tcrConfigs.ResignedRuleSkippedIdsStr) == false)
        {
            tcrConfigs.ResignedRuleSkippedIds = UtilityFunctions.GetIntListFromString(tcrConfigs.ResignedRuleSkippedIdsStr);
        }

        var lookupsAgents = new List<LookupItem<int>>();

        if (item.FirstAgentId > 0)
        {
            lookupsAgents.Add(new LookupItem<int>(item.FirstAgentName ?? "", item.FirstAgentId, "", 0));
        }

        if (item.SecondAgentId > 0)
        {
            lookupsAgents.Add(new LookupItem<int>(item.SecondAgentName ?? "", item.SecondAgentId, "", 0));
        }
        if (item.ThirdAgentId > 0)
        {
            lookupsAgents.Add(new LookupItem<int>(item.ThirdAgentName ?? "", item.ThirdAgentId, "", 0));
        }
        tcrConfigs.AgentsList = lookupsAgents.ToArray();
        item.TcrConfigs = tcrConfigs;
        return item;
    }

    private IQueryable<VwPrimeTcr> SetAgentsFilters(IQueryable<VwPrimeTcr> rawQuery, PrimeTcrsFiltersModel filters)
    {

        if (filters.AgentId == 0 && filters.ManagersIds.Any() == false)
        {
            return rawQuery;
        }
        if (filters.AgentId > 0 && filters.ManagersIds.Any() == false)
        {
            rawQuery = rawQuery.Where(z => z.AgentsIdsArray.Contains(filters.AgentId));

            return rawQuery;
        }

        if (filters.AgentId == 0 && filters.ManagersIds.Any())
        {
            rawQuery = rawQuery.Where(z => z.ManagersIdsArray.Any(r => filters.ManagersIds.Contains(r)));

            return rawQuery;
        }


        if (filters.UserAgentOr)
        {
            rawQuery = rawQuery.Where(z => (z.ManagersIdsArray.Any(r => filters.ManagersIds.Contains(r)) || z.AgentsIdsArray.Contains(filters.AgentId)));
        }
        else
        {
            rawQuery = rawQuery.Where(z => (z.ManagersIdsArray.Any(r => filters.ManagersIds.Contains(r)) && z.AgentsIdsArray.Contains(filters.AgentId)));
        }

        return rawQuery;

    }

    private IQueryable<VwPrimeTcr> SetConfirmationDateFilters(IQueryable<VwPrimeTcr> rawQuery,
        PrimeTcrsFiltersModel filters)
    {
        var confirmedFromDate = 20150101;
        var confirmedToDate = UtilityFunctions.GetIntegerFromDate(DateTime.UtcNow);

        if (filters.ConfirmContractDateFrom != null)
        {
            confirmedFromDate = UtilityFunctions.GetIntegerFromDate(filters.ConfirmContractDateFrom);
        }

        if (filters.ConfirmContractDateTo != null)
        {
            confirmedToDate = UtilityFunctions.GetIntegerFromDate(filters.ConfirmContractDateTo);
        }

        if (filters.ConfirmContractDateFrom != null || filters.ConfirmContractDateTo != null)
        {
            if (filters.IncludeHalfConfirmed)
            {
                rawQuery = rawQuery.Where(z =>
                    (z.ConfirmedContractingDateNumeric > 0 && z.ConfirmedContractingDateNumeric >= confirmedFromDate &&
                     z.ConfirmedContractingDateNumeric <= confirmedToDate) ||
                    (z.ConfirmedHalfContractingDateNumeric > 0 &&
                     z.ConfirmedHalfContractingDateNumeric >= confirmedFromDate &&
                     z.ConfirmedHalfContractingDateNumeric <= confirmedToDate));
            }
            else
            {
                var confirmedContracted = (int)PrimeTcrStatuses.ConfirmedContracted;

                rawQuery = rawQuery.Where(z =>
                    z.ConfirmedContractingDateNumeric > 0 && z.ConfirmedContractingDateNumeric >= confirmedFromDate &&
                    z.ConfirmedContractingDateNumeric <= confirmedToDate && z.PrimeTcrStatusId == confirmedContracted);
            }
        }

        return rawQuery;
    }

    private IQueryable<VwPrimeTcr> SetSpecialFilters(IQueryable<VwPrimeTcr> rawQuery, PrimeTcrsFiltersModel filters)
    {
        var lst = filters.EntityStatusIds.ToArray();
        var confirmedContracted = (int)PrimeTcrStatuses.ConfirmedContracted;
        var applyHalf = lst.Length == 1 && filters.EntityStatusIds.First() == confirmedContracted;

        if (lst.Length > 0 && applyHalf == false && filters.IncludeHalfCommission == false &&
           filters.IncludeHalfConfirmed == false)
        {
            rawQuery = rawQuery.Where(z => lst.Contains(z.PrimeTcrStatusId));

            return rawQuery;
        }

        var excludeList = new[] { (int)PrimeTcrStatuses.CanceledByDeveloper, (int)PrimeTcrStatuses.Deleted };

        if (applyHalf)
        {
            rawQuery = rawQuery.Where(z =>
                excludeList.Contains(z.PrimeTcrStatusId) == false && (z.HalfCommissionPaid || z.IsHalfContracted ||
                                                                      z.PrimeTcrStatusId == confirmedContracted));
        }
        else if (filters.IncludeHalfCommission)
        {
            rawQuery = rawQuery.Where(z => excludeList.Contains(z.PrimeTcrStatusId) == false && z.HalfCommissionPaid);
        }
        else if (filters.IncludeHalfConfirmed)
        {
            rawQuery = rawQuery.Where(z => excludeList.Contains(z.PrimeTcrStatusId) == false && z.IsHalfContracted);
        }

        return rawQuery;
    }

    private IQueryable<VwPrimeTcr> SetIdsFilters(IQueryable<VwPrimeTcr> rawQuery, PrimeTcrsFiltersModel filters)
    {
        if (string.IsNullOrEmpty(filters.LeadTicketsIds) == false)
        {
            var ids = UtilityFunctions.GetLongListFromString(filters.LeadTicketsIds);

            if (ids.Any())
            {
                rawQuery = rawQuery.Where(z => ids.Contains(z.LeadTicketId));
            }
        }

        if (string.IsNullOrEmpty(filters.EntityIds) == false)
        {
            var ids = UtilityFunctions.GetLongListFromString(filters.EntityIds);

            if (ids.Any())
            {
                rawQuery = rawQuery.Where(z => ids.Contains(z.Id));
            }
        }

        if (string.IsNullOrEmpty(filters.UnitId) == false)
        {
            rawQuery = rawQuery.Where(z => z.UnitNumberLowered == filters.UnitId.Trim()
                .ToLower());
        }

        return rawQuery;
    }

    private IQueryable<VwPrimeTcr> SetSimpleDateFilters(IQueryable<VwPrimeTcr> rawQuery, PrimeTcrsFiltersModel filters)
    {
        rawQuery = ApplyDateFilter(rawQuery, filters.EntityCreationDateFrom, filters.EntityCreationDateTo,
            z => z.CreationDateNumeric);

        rawQuery = ApplyDateFilter(rawQuery, filters.LastReviewedDateFrom, filters.LastReviewedDateTo,
            z => z.LastDeveloperReviewingDateNumeric);

        rawQuery = ApplyDateFilter(rawQuery, filters.LastConflictDateFrom, filters.LastConflictDateTo,
            z => z.LastConflictDateNumeric);

        return rawQuery;
    }

    private IQueryable<VwPrimeTcr> SetComplexDateFilters(IQueryable<VwPrimeTcr> rawQuery, PrimeTcrsFiltersModel filters)
    {
        var expressions = new List<Expression<Func<VwPrimeTcr, bool>>>();

        var filterOne = BuildDateFilter(filters.RecContractDateFrom, filters.RecContractDateTo,
            z => z.RecCloseDateNumeric);

        if (filterOne != null)
        {
            expressions.Add(filterOne);
        }

        var filterTwo = BuildDateFilter(filters.RecReserveDateFrom, filters.RecReserveDateTo,
            z => z.RecReserveDateNumeric);

        if (filterTwo != null)
        {
            expressions.Add(filterTwo);
        }

        var filterThree = BuildDateFilter(filters.LastResolvedDateFrom, filters.LastResolvedDateTo,
            z => z.LastResolveDateNumeric);

        if (filterThree != null)
        {
            expressions.Add(filterThree);
        }

        var filterFour = BuildDateFilter(filters.PostponeDateFrom, filters.PostponeDateTo,
            z => z.LastPostponeDateNumeric);

        if (filterFour != null)
        {
            expressions.Add(filterFour);
        }

        if (expressions.Count == 0)
        {
            return rawQuery;
        }

        // Combine filters using OR
        Expression<Func<VwPrimeTcr, bool>> combinedFilter = expressions.First();

        for (var i = 1; i < expressions.Count; i++)
        {
            combinedFilter = CombineExpressions(combinedFilter, expressions[i], Expression.OrElse);
        }

        Expression<Func<VwPrimeTcr, bool>>? existingFilter = null;

        if (rawQuery.Expression is MethodCallExpression methodCall && methodCall.Method.Name == "Where")
        {
            if (methodCall.Arguments[1] is UnaryExpression unaryExpression &&
               unaryExpression.Operand is LambdaExpression lambdaExpression)
            {
                existingFilter = (Expression<Func<VwPrimeTcr, bool>>)lambdaExpression;
            }
        }

        if (existingFilter != null)
        {
            var finalFilter = CombineExpressions(existingFilter, combinedFilter, Expression.AndAlso);
            rawQuery = rawQuery.Where(finalFilter);
        }
        else
        {
            // No existing filter, apply the combined filter directly
            rawQuery = rawQuery.Where(combinedFilter);
        }

        return rawQuery;
    }

    private Expression<Func<VwPrimeTcr, bool>>? BuildDateFilter(DateTime? fromDate, DateTime? toDate,
       Expression<Func<VwPrimeTcr, int>> dateFieldSelector)
    {
        Expression<Func<VwPrimeTcr, bool>>? filter = null;
        var parameter = Expression.Parameter(typeof(VwPrimeTcr), "z"); // Create a fresh parameter

        // Replace the parameter in dateFieldSelector with the new one
        var body = new ParameterReplacer(dateFieldSelector.Parameters[0], parameter).Visit(dateFieldSelector.Body);

        if (fromDate != null)
        {
            var fromNumeric = UtilityFunctions.GetIntegerFromDate(fromDate);

            var fromFilter = Expression.Lambda<Func<VwPrimeTcr, bool>>(
                Expression.GreaterThanOrEqual(body, Expression.Constant(fromNumeric)), parameter);

            filter = filter == null ? fromFilter : CombineExpressions(filter, fromFilter, Expression.AndAlso);
        }

        if (toDate != null)
        {
            var toNumeric = UtilityFunctions.GetIntegerFromDate(toDate);

            var toFilter = Expression.Lambda<Func<VwPrimeTcr, bool>>(
                Expression.LessThanOrEqual(body, Expression.Constant(toNumeric)), parameter);

            filter = filter == null ? toFilter : CombineExpressions(filter, toFilter, Expression.AndAlso);
        }

        return filter;
    }

    private Expression<Func<VwPrimeTcr, bool>> CombineExpressions<VwPrimeTcr>(Expression<Func<VwPrimeTcr, bool>> first,
        Expression<Func<VwPrimeTcr, bool>> second, Func<Expression, Expression, BinaryExpression> merge)
    {
        var parameter = Expression.Parameter(typeof(VwPrimeTcr));
        var left = new ReplaceParameterVisitor(first.Parameters[0], parameter).Visit(first.Body);
        var right = new ReplaceParameterVisitor(second.Parameters[0], parameter).Visit(second.Body);

        return Expression.Lambda<Func<VwPrimeTcr, bool>>(merge(left, right), parameter);
    }

    private IQueryable<VwPrimeTcr> ApplyDateFilter(
     IQueryable<VwPrimeTcr> query, DateTime? fromDate, DateTime? toDate,
     Expression<Func<VwPrimeTcr, int>> dateFieldSelector)
    {
        if (fromDate.HasValue)
        {
            var fromNumeric = UtilityFunctions.GetIntegerFromDate(fromDate);
            query = query.Where(Expression.Lambda<Func<VwPrimeTcr, bool>>(
                Expression.AndAlso(
                    Expression.GreaterThan(dateFieldSelector.Body, Expression.Constant(0)),
                    Expression.GreaterThanOrEqual(dateFieldSelector.Body, Expression.Constant(fromNumeric))
                ),
                dateFieldSelector.Parameters
            ));
        }

        if (toDate.HasValue)
        {
            var toNumeric = UtilityFunctions.GetIntegerFromDate(toDate);
            query = query.Where(Expression.Lambda<Func<VwPrimeTcr, bool>>(
                Expression.AndAlso(
                    Expression.GreaterThan(dateFieldSelector.Body, Expression.Constant(0)),
                    Expression.LessThanOrEqual(dateFieldSelector.Body, Expression.Constant(toNumeric))
                ),
                dateFieldSelector.Parameters
            ));
        }

        return query;
    }


    private IQueryable<VwPrimeTcr> SetNumericFilters(IQueryable<VwPrimeTcr> rawQuery, PrimeTcrsFiltersModel filters)
    {
        rawQuery = ApplyConditionalFilter(rawQuery, filters.CompanyId > 0,
            filters.UseTcrCompany
                ? (Expression<Func<VwPrimeTcr, bool>>)(z => z.TcrCompanyId == filters.CompanyId)
                : z => z.LeadTicketCompanyId == filters.CompanyId);

        rawQuery = ApplyConditionalFilter(rawQuery, filters.BranchId > 0,
            filters.UseTcrCompany
                ? (Expression<Func<VwPrimeTcr, bool>>)(z => z.TcrBranchId == filters.BranchId)
                : z => z.LeadTicketBranchId == filters.BranchId);

        rawQuery = ApplyConditionalFilter(rawQuery, filters.UsageId > 0, z => z.UsageId == filters.UsageId);
        rawQuery = ApplyConditionalFilter(rawQuery, filters.DeveloperId > 0, z => z.DeveloperId == filters.DeveloperId);
        rawQuery = ApplyConditionalFilter(rawQuery, filters.ProjectId > 0, z => z.ProjectId == filters.ProjectId);

        rawQuery = ApplyConditionalFilter(rawQuery, filters.PropertyTypeId > 0,
            z => z.PropertyTypeId == filters.PropertyTypeId);

        rawQuery = ApplyConditionalFilter(rawQuery, filters.AgencyId > 0, z => z.MarketingAgencyId == filters.AgencyId);

        rawQuery = ApplyConditionalFilter(rawQuery, filters.LeadSourceId > 0,
            z => z.LeadSourceId == filters.LeadSourceId);

        rawQuery = ApplyConditionalFilter(rawQuery, filters.KnowSourceId > 0,
            z => z.LastMarketingChannelId == filters.KnowSourceId);

        rawQuery = ApplyConditionalFilter(rawQuery, filters.KnowSubSourceId > 0,
            z => z.LastMarketingChannelExtraId == filters.KnowSubSourceId);

        rawQuery = ApplyConditionalFilter(rawQuery, filters.ClosingChannelId > 0,
            z => z.ClosingChannelId == filters.ClosingChannelId);

        rawQuery = ApplyConditionalFilter(rawQuery, filters.ClosingSubChannelId > 0,
            z => z.ClosingChannelExtraId == filters.ClosingSubChannelId);

        rawQuery = ApplyConditionalFilter(rawQuery, filters.WasOldId > 0, z => z.WasOld == (filters.WasOldId == 1));

        rawQuery = ApplyConditionalFilter(rawQuery, filters.CollectedId > 0,
            z => z.IsCompanyCommissionCollected == (filters.CollectedId == 1));

        rawQuery = ApplyConditionalFilter(rawQuery, filters.IsCorporateId > 0,
            z => z.IsCorporate == (filters.IsCorporateId == 1));

        rawQuery = ApplyConditionalFilter(rawQuery, filters.HalfCommissionCollectedStatusId > 0,
            z => z.HalfCommissionPaid == (filters.HalfCommissionCollectedStatusId == 1) && z.IsHalfCommission);

        rawQuery = ApplyConditionalFilter(rawQuery, filters.InvoicedId > 0,
            z => z.Invoiced == (filters.InvoicedId == 1));

        rawQuery = ApplyConditionalFilter(rawQuery, filters.VerificationStatusId > -1,
            z => z.VerificationStatusId == filters.VerificationStatusId);

        rawQuery = ApplyConditionalFilter(rawQuery, filters.CountryId > 0, z => z.CountryId == filters.CountryId);
        rawQuery = ApplyConditionalFilter(rawQuery, filters.CityId > 0, z => z.CityId == filters.CityId);

        rawQuery = ApplyConditionalFilter(rawQuery, filters.NeighborhoodId > 0,
            z => z.NeighborhoodId == filters.NeighborhoodId);

        rawQuery = ApplyConditionalFilter(rawQuery, filters.DistrictId > 0, z => z.DistrictId == filters.DistrictId);

        return rawQuery;
    }

    private IQueryable<VwPrimeTcr> ApplyConditionalFilter(IQueryable<VwPrimeTcr> query, bool condition,
        Expression<Func<VwPrimeTcr, bool>> filter)
    {
        return condition ? query.Where(filter) : query;
    }
}

public class ReplaceParameterVisitor : ExpressionVisitor
{
    private readonly ParameterExpression _newParameter;
    private readonly ParameterExpression _oldParameter;

    public ReplaceParameterVisitor(ParameterExpression oldParameter, ParameterExpression newParameter)
    {
        _oldParameter = oldParameter;
        _newParameter = newParameter;
    }

    protected override Expression VisitParameter(ParameterExpression node)
    {
        // Replace the old parameter with the new parameter
        return node == _oldParameter ? _newParameter : base.VisitParameter(node);
    }
}
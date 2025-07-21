using CloudBlue.Domain.DataModels.Crm;
using CloudBlue.Domain.DomainModels;
using CloudBlue.Domain.DomainModels.CallLeads;
using CloudBlue.Domain.DomainModels.Filtration.Interfaces;
using CloudBlue.Domain.DomainModels.Filtration.JsonFilters;
using CloudBlue.Domain.Enums;
using CloudBlue.Domain.GenericTypes;
using CloudBlue.Domain.Interfaces.DbContext;
using CloudBlue.Domain.Interfaces.Repositories;
using CloudBlue.Domain.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace CloudBlue.Data.Repositories.Crm;

public class CallsRepository(ICrmDataContext appDb) : ICallsRepository
{
    public int CurrentUserId { get; set; }
    public int CurrentUserBranchId { get; set; }
    public int CurrentUserCompanyId { get; set; }

    public long LastCreatedItemId { get; set; }

    public async Task<ListResult<CallItemForList>> GetCalls(CallsFiltersModel filters)
    {
    var rawCalls = appDb.VwCalls.AsQueryable();

    #region Filters

    #region Client

    if (string.IsNullOrEmpty(filters.ClientName) == false)
    {
    rawCalls = rawCalls.Where(z =>
        EF.Functions.Like(z.ClientNameLowered, $"%{filters.ClientName.Trim().ToLower()}%"));
    }

    if (string.IsNullOrEmpty(filters.ClientNameArabic) == false)
    {
    rawCalls = rawCalls.Where(z =>
        z.ClientNameArabic != null && z.ClientNameArabic == filters.ClientNameArabic);
    }

    if (string.IsNullOrEmpty(filters.ClientContactDevice) == false)
    {
    var jsonFilter = new[] { new ClientDeviceInfoFilter(filters.ClientContactDevice) };
    var deviceFilter = UtilityFunctions.SerializeToJsonString(jsonFilter);

    rawCalls = rawCalls.Where(e =>
        e.ContactDevicesJsonb != null && EF.Functions.JsonContains(e.ContactDevicesJsonb, deviceFilter));
    }

    if (filters.InternationalOnly > 0)
    {
    var flag = filters.InternationalOnly != 1;
    var jsonFilter = new[] { new ClientCountryInfoFilter("0020") };
    var countryFilter = UtilityFunctions.SerializeToJsonString(jsonFilter);

    rawCalls = rawCalls.Where(e =>
        e.ContactDevicesJsonb != null &&
        EF.Functions.JsonContains(e.ContactDevicesJsonb, countryFilter) == flag);
    }

    if (filters.ClientCategoryId > 0)
    {
    rawCalls = rawCalls.Where(z => z.ClientCategoryId == filters.ClientCategoryId);
    }

    #endregion

    #region Entity

    if (string.IsNullOrEmpty(filters.EntityIds) == false)
    {
    var ids = filters.EntityIds.Split(',')
        .Select(long.Parse)
        .ToArray();

    rawCalls = rawCalls.Where(z => ids.Contains(z.Id));
    }

    if (filters.EntityStatusIds.Any())
    {
    var ids = filters.EntityStatusIds.Select(z => (CallStatuses)z)
        .ToArray();

    rawCalls = rawCalls.Where(z => ids.Contains(z.CallStatusId));
    }

    if (filters.CompanyId > 0)
    {
    rawCalls = rawCalls.Where(z => z.CompanyId == filters.CompanyId);
    }

    if (filters.BranchId > 0)
    {
    rawCalls = rawCalls.Where(z => z.BranchId == filters.BranchId);
    }

    if (filters.CreatedById > 0)
    {
    rawCalls = rawCalls.Where(z => z.CreatedById == filters.CreatedById);
    }

    if (filters.EntityCreationDateFrom != null)
    {
    var dateNumeric = int.Parse(filters.EntityCreationDateFrom.Value.ToString("yyyyMMdd"));
    rawCalls = rawCalls.Where(z => z.CreationDateNumeric > 0 && z.CreationDateNumeric >= dateNumeric);
    }

    if (filters.EntityCreationDateTo != null)
    {
    var dateNumeric = int.Parse(filters.EntityCreationDateTo.Value.ToString("yyyyMMdd"));
    rawCalls = rawCalls.Where(z => z.CreationDateNumeric > 0 && z.CreationDateNumeric <= dateNumeric);
    }

    #endregion

    #region Lead Source

    if (filters.LeadSourceId > 0)
    {
    rawCalls = rawCalls.Where(z => z.LeadSourceId == filters.LeadSourceId);
    }

    if (filters.KnowSourceId > 0)
    {
    rawCalls = rawCalls.Where(z => z.KnowSourceId == filters.KnowSourceId);
    }

    if (filters.KnowSubSourceId > 0)
    {
    rawCalls = rawCalls.Where(z => z.KnowSourceExtraId == filters.KnowSubSourceId);
    }

    #endregion

    #region Call

    if (filters.CallTypeId > 0)
    {
    rawCalls = rawCalls.Where(z => z.CallTypeId == (CallTypes)filters.CallTypeId);
    }

    #endregion

    #endregion

    var retObj = new ListResult<CallItemForList>();
    retObj.TotalCount = await rawCalls.CountAsync();
    var sortingExpression = GetSortingExpression(filters.SortField, filters.SortDirection);

    var rawItems = await rawCalls.OrderBy(sortingExpression)
        .Skip(filters.PageIndex * filters.PageSize)
        .Select(z => new
        {
            z.Id,
            z.CreationDate,
            z.BranchName,
            z.CallType,
            z.ClientName,
            z.ClientNameArabic,
            z.Location,
            z.ContactDevicesJsonb,
            z.CanceledDate,
            z.CanceledBy,
            z.CallStatusId,
            z.CallTypeId,
            z.CompanyName,
            z.CallNote,
            z.CreatedBy,
            z.Usage,
            z.SalesType,
            z.ClientId,
            z.DurationInSeconds,
            z.HandledBy,
            z.HandledDate,
            z.IsArchived,
            z.KnowSource,
            z.KnowSubSource,
            z.LeadSource,
            z.RecentEventsJsonb,
            z.StatusReason,
            z.CallStatus,
            z.DurationStr,
            z.IsPotential,
            z.ClientTitle,
            z.ClientType,
            z.PropertyType,
            z.IsVip,
            z.CallStatusBackgroundColor,
            z.CallStatusFontColor,
            z.CallTypeBackgroundColor,
            z.CallTypeFontColor,
            z.Gender,
            z.WorkField,
            z.ClientCategory,
            z.ClientOccupation,
            z.ClientCompanyName,
            z.ClientBirthDate
        })
        .Take(filters.PageSize)
        .ToArrayAsync();

    var items = new CallItemForList[rawItems.Length];

    for (var idx = 0; idx < rawItems.Length; idx++)
    {
    //rawItem
    var rawItem = rawItems[idx];

    var item = new CallItemForList
    {
        Id = rawItem.Id,
        CreationDate = rawItem.CreationDate,
        BranchName = rawItem.BranchName,
        CallType = rawItem.CallType,
        ClientName = rawItem.ClientName,
        ClientNameArabic = rawItem.ClientNameArabic,
        Location = rawItem.Location,
        CanceledDate = rawItem.CanceledDate,
        CanceledBy = rawItem.CanceledBy,
        CallStatusId = rawItem.CallStatusId,
        CallTypeId = rawItem.CallTypeId,
        CompanyName = rawItem.CompanyName,
        CallNote = rawItem.CallNote,
        CreatedBy = rawItem.CreatedBy,
        Usage = rawItem.Usage,
        SalesType = rawItem.SalesType,
        ClientId = rawItem.ClientId,
        DurationInSeconds = rawItem.DurationInSeconds,
        HandledBy = rawItem.HandledBy,
        HandledDate = rawItem.HandledDate,
        IsArchived = rawItem.IsArchived,
        KnowSource = rawItem.KnowSource,
        KnowSubSource = rawItem.KnowSubSource,
        LeadSource = rawItem.LeadSource,
        StatusReason = rawItem.StatusReason,
        CallStatus = rawItem.CallStatus,
        DurationStr = rawItem.DurationStr,
        IsPotential = rawItem.IsPotential,
        ClientTitle = rawItem.ClientTitle,
        ClientType = rawItem.ClientType,
        PropertyType = rawItem.PropertyType,
        IsVip = rawItem.IsVip,
        CallStatusBackgroundColor = rawItem.CallStatusBackgroundColor,
        CallStatusFontColor = rawItem.CallStatusFontColor,
        CallTypeBackgroundColor = rawItem.CallTypeBackgroundColor,
        CallTypeFontColor = rawItem.CallTypeFontColor,
        Gender = rawItem.Gender,
        WorkField = rawItem.WorkField,
        ClientCategory = rawItem.ClientCategory,
        ClientOccupation = rawItem.ClientOccupation,
        ClientCompanyName = rawItem.ClientCompanyName,
        ClientBirthDate = rawItem.ClientBirthDate
    };

    if (rawItem.ContactDevicesJsonb != null)
    {
    var devices = UtilityFunctions.DeserializeJsonDocument<ClientPhoneItem[]>(rawItem.ContactDevicesJsonb);

    if (devices.Length > 0)
    {
    item.ClientContactDevices = devices;

    item.ClientPhone = string.Join(", ", devices.OrderBy(z => z.DeviceType)
        .Select(z => z.DeviceInfo)
        .ToArray());
    }
    }

    if (rawItem.RecentEventsJsonb != null)
    {
    var events = UtilityFunctions.DeserializeJsonDocument<SystemEventItem[]>(rawItem.RecentEventsJsonb);

    if (events.Length > 0)
    {
    item.SystemEvents = events;
    }
    }

    items[idx] = item;
    }

    retObj.Items = items;

    return retObj;
    }

    public async Task<bool> CreateCallAsync(CallCreateModel model)
    {
    var call = new Call
    {
        ClientId = model.ClientId,
        BranchId = model.BranchId,
        CallType = (CallTypes)model.CallTypeId,
        CompanyId = model.CompanyId,
        CallStatus = (CallStatuses)model.CallStatusId,
        CallNote = model.CallComment ?? string.Empty,
        LeadSourceId = model.LeadSourceInfo.LeadSourceId,
        KnowSourceId = model.LeadSourceInfo.KnowSourceId,
        KnowSourceExtraId = model.LeadSourceInfo.KnowSubSourceId,
        SourceExtra = model.LeadSourceInfo.LeadSourceExtra ?? string.Empty,
        CreationDate = DateTime.UtcNow,
        CreationDateNumeric = int.Parse(DateTime.UtcNow.ToString("yyyyMMdd")),
        CreatedById = CurrentUserId,
        DurationInSeconds = model.DurationInSeconds,
        CallTypeOther = string.Empty,
        HandledById = 0,
        CanceledById = 0,
        HandledDateNumeric = 0,
        CanceledDateNumeric = 0,
        HandledDate = null,
        CanceledDate = null,
        LastEventId = 0,
        IsArchived = false,
        VoidReasonId = 0,
        ExpRegId = 0,
        CampaignOwnerId = 0,
        WebLeadId = 0,
        CollectiveCampaignId = 0,
        ProjectCampaignId = 0,
        CreatedBy = model.CurrentUserName,
        HandledBy = string.Empty,
        CanceledBy = string.Empty,
        Location = model.LocationStr,
        DurationStr = model.Duration,
        StatusReason = model.StatusReason
    };

    await appDb.Calls.AddAsync(call);
    await appDb.SaveChangesAsync();
    LastCreatedItemId = call.Id;

    return true;
    }

    private string GetSortingExpression(string sortField, string sortDirection)
    {
    if (string.IsNullOrEmpty(sortDirection))
    {
    sortDirection = "desc";
    }

    if (string.IsNullOrEmpty(sortField))
    {
    sortField = "Id";
    }
    else
    {
    sortField = sortField.Replace("CreationDate", "CreationDateNumeric");
    }

    return $"{sortField} {sortDirection}";
    }
}
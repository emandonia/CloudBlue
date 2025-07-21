using CloudBlue.Domain.BaseTypes;
using CloudBlue.Domain.DataModels;
using CloudBlue.Domain.DataModels.Crm;
using CloudBlue.Domain.DataModels.PrimeTcrs;
using CloudBlue.Domain.DomainModels;
using CloudBlue.Domain.DomainModels.CallLeads;
using CloudBlue.Domain.DomainModels.Users.UserSessions;
using CloudBlue.Domain.Enums;
using CloudBlue.Domain.Interfaces.Repositories;
using CloudBlue.Domain.Interfaces.Services;
using CloudBlue.Domain.Utilities;

namespace CloudBlue.BusinessServices.App;

public class SystemEventsService(ISystemEventsRepository repo, LoggedInUserInfo loggedInUserInfo, ICachingService cache)
    : BaseService, ISystemEventsService
{
    private SystemEventTemplate[] _systemEventTemplates = [];
    public async Task AddEventAsync(long entityId, EntityTypes entityType, EventTypes eventType, long clientId, int userId, string comment, EventProcesses eventProcess, int contactingTypeId)
    {

        var eventItem = new SystemEvent
        {
            ClientId = clientId,
            ContactingTypeId = contactingTypeId,
            EntityId = entityId,
            UserId = userId,
            EventTypeId = (int)eventType,
            EntityTypeId = (int)entityType,
            EventCreationDateTime = DateTime.UtcNow,
            EventCreationDateTimeNumeric = long.Parse(DateTime.UtcNow.ToString("yyyyMMddHHmmss")),
            EventDateTime = DateTime.UtcNow,
            EventDateTimeNumeric = long.Parse(DateTime.UtcNow.ToString("yyyyMMddHHmmss")),
            EventProcessId = (int)eventProcess,
            EventComment = comment,
            Dismissed = false,
            EventSource = false,
            FromUserId = 0,
            Impersonated = false,
            IsConverted = false,
            OriginalUserId = userId,
            ToUserId = 0
        };

        await repo.CreateEventsAsync(new List<SystemEvent>() { eventItem });
        await repo.UpdateEntityEventsAsync(entityType, entityId);


    }

    public async Task GenerateNewLeadTicketEventsAsync(LeadTicketCreateModel model)
    {
        //create lead
        var template = _systemEventTemplates.FirstOrDefault(x => x.EventTemplateId == EventTemplates.LeadTicketCreate);
        var events = new List<SystemEvent>();

        if (template != null)
        {
            events.Add(new SystemEvent
            {
                ClientId = model.ClientId,
                ContactingTypeId = 0,
                EntityId = model.LeadTicketId,
                UserId = loggedInUserInfo.UserId,
                EventTypeId = template.EventTypeId,
                EntityTypeId = template.EntityTypeId,
                EventCreationDateTime = DateTime.UtcNow,
                EventCreationDateTimeNumeric = long.Parse(DateTime.UtcNow.ToString("yyyyMMddHHmmss")),
                EventDateTime = DateTime.UtcNow,
                EventDateTimeNumeric = long.Parse(DateTime.UtcNow.ToString("yyyyMMddHHmmss")),
                EventProcessId = template.EventProcessId,
                EventComment = GetEventComment(template, model),
                Dismissed = false,
                EventSource = false,
                FromUserId = 0,
                Impersonated = false,
                IsConverted = false,
                OriginalUserId = loggedInUserInfo.UserId,
                ToUserId = 0
            });
        }

        if (model.BranchId > 0)
        {
            template = _systemEventTemplates.FirstOrDefault(x =>
                x.EventTemplateId == EventTemplates.LeadTicketAssignToBranch);

            if (template != null)
            {
                events.Add(new SystemEvent
                {
                    ClientId = model.ClientId,
                    ContactingTypeId = 0,
                    EntityId = model.LeadTicketId,
                    UserId = loggedInUserInfo.UserId,
                    EventTypeId = template.EventTypeId,
                    EntityTypeId = template.EntityTypeId,
                    EventCreationDateTime = DateTime.UtcNow,
                    EventCreationDateTimeNumeric = long.Parse(DateTime.UtcNow.ToString("yyyyMMddHHmmss")),
                    EventDateTime = DateTime.UtcNow,
                    EventDateTimeNumeric = long.Parse(DateTime.UtcNow.ToString("yyyyMMddHHmmss")),
                    EventProcessId = template.EventProcessId,
                    EventComment = GetEventComment(template, model),
                    Dismissed = false,
                    EventSource = false,
                    FromUserId = 0,
                    Impersonated = false,
                    IsConverted = false,
                    OriginalUserId = loggedInUserInfo.UserId,
                    ToUserId = 0
                });
            }
        }

        if (model.AgentId > 0)
        {
            template = _systemEventTemplates.FirstOrDefault(x =>
                x.EventTemplateId == EventTemplates.LeadTicketAssignToAgent);

            if (template != null)
            {
                events.Add(new SystemEvent
                {
                    ClientId = model.ClientId,
                    ContactingTypeId = 0,
                    EntityId = model.LeadTicketId,
                    UserId = loggedInUserInfo.UserId,
                    EventTypeId = template.EventTypeId,
                    EntityTypeId = template.EntityTypeId,
                    EventCreationDateTime = DateTime.UtcNow,
                    EventCreationDateTimeNumeric = long.Parse(DateTime.UtcNow.ToString("yyyyMMddHHmmss")),
                    EventDateTime = DateTime.UtcNow,
                    EventDateTimeNumeric = long.Parse(DateTime.UtcNow.ToString("yyyyMMddHHmmss")),
                    EventProcessId = template.EventProcessId,
                    EventComment = GetEventComment(template, model),
                    Dismissed = false,
                    EventSource = false,
                    FromUserId = 0,
                    Impersonated = false,
                    IsConverted = false,
                    OriginalUserId = loggedInUserInfo.UserId,
                    ToUserId = 0
                });
            }
        }

        await repo.CreateEventsAsync(events);
        await repo.UpdateEntityEventsAsync(EntityTypes.LeadTicket, model.LeadTicketId);
    }

    public async Task GenerateLeadTicketsActionsEventsAsync(List<LeadTicket> affectedLeads, LeadTicketActionModel model)
    {
        EventTemplates? eventTemplate = null;

        switch (model.Action)
        {
            case SystemPrivileges.LeadTicketsAssignToAgent:
                eventTemplate = EventTemplates.LeadTicketAssignToAgent;

                break;

            case SystemPrivileges.LeadTicketsReAssignToAgent:
                eventTemplate = EventTemplates.LeadTicketReAssignToAgent;

                break;

            case SystemPrivileges.LeadTicketsAssignToBranch:
                eventTemplate = EventTemplates.LeadTicketAssignToBranch;

                break;

            case SystemPrivileges.LeadTicketsReAssignToBranch:
                eventTemplate = EventTemplates.LeadTicketReAssignToBranch;

                break;

            case SystemPrivileges.LeadTicketsMoveToCompany:
                eventTemplate = EventTemplates.LeadTicketMoveToCompany;

                break;

            case SystemPrivileges.LeadTicketsArchive:
                eventTemplate = EventTemplates.LeadTicketArchive;

                break;

            case SystemPrivileges.LeadTicketsUnArchive:
                eventTemplate = EventTemplates.LeadTicketUnArchive;

                break;

            case SystemPrivileges.LeadTicketsSetVoid:
                eventTemplate = EventTemplates.LeadTicketSetVoid;

                break;

            case SystemPrivileges.LeadTicketsReject:
                eventTemplate = EventTemplates.LeadTicketReject;

                break;
        }

        if (eventTemplate == null && model.SkipTemplate == false)
        {
            return;
        }

        SystemEventTemplate? template = null;

        if (model.SkipTemplate == false)
        {
            template = _systemEventTemplates.FirstOrDefault(x => x.EventTemplateId == eventTemplate);
        }

        if (template == null && model.SkipTemplate == false)
        {
            return;
        }

        var events = new List<SystemEvent>();
        var contactingTypeId = model.ContactingTypeId;
        var eventTypeId = (int)EventTypes.LeadTicketFollowUp;
        var entityTypeId = (int)EntityTypes.LeadTicket;
        var eventProcessId = (int)EventProcesses.FollowUp;
        var eventComment = model.Comment;
        var eventDateTime = DateTime.UtcNow;

        if (model.SkipTemplate && model.ReminderDate != null)
        {
            eventDateTime = UtilityFunctions.GetUtcDateTime(model.ReminderDate.Value);
        }

        if (template != null)
        {
            contactingTypeId = 0;
            eventTypeId = template.EventTypeId;
            entityTypeId = template.EntityTypeId;
            eventProcessId = template.EventProcessId;
            eventComment = GetEventComment(template, model);
        }

        foreach (var affectedLead in affectedLeads)
        {
            var fromUser = 0;
            var toUser = 0;

            if (model.Action == SystemPrivileges.LeadTicketsReAssignToAgent)
            {
                fromUser = affectedLead.LeadTicketExtension.OldAgentId;
                toUser = affectedLead.CurrentAgentId;
            }

            events.Add(new SystemEvent
            {
                ClientId = affectedLead.ClientId,
                ContactingTypeId = contactingTypeId,
                EntityId = affectedLead.Id,
                UserId = loggedInUserInfo.UserId,
                EventTypeId = eventTypeId,
                EntityTypeId = entityTypeId,
                EventCreationDateTime = DateTime.UtcNow,
                EventCreationDateTimeNumeric = long.Parse(DateTime.UtcNow.ToString("yyyyMMddHHmmss")),
                EventDateTime = eventDateTime,
                EventDateTimeNumeric = long.Parse(eventDateTime.ToString("yyyyMMddHHmmss")),
                EventProcessId = eventProcessId,
                EventComment = eventComment,
                Dismissed = false,
                EventSource = false,
                FromUserId = fromUser,
                Impersonated = false,
                IsConverted = false,
                OriginalUserId = loggedInUserInfo.UserId,
                ToUserId = toUser
            });
        }

        await repo.CreateBulkEventsAsync(events);

        foreach (var affectedLead in affectedLeads)
        {
            await repo.UpdateEntityEventsAsync(EntityTypes.LeadTicket, affectedLead.Id, model.CurrentAgentId);
        }
    }

    public async Task
        GenerateLeadTicketsConvertedToTcrAsync(long leadTicketId, EntityTypes tcrType, long clientId)
    {
        var eventTemplate = tcrType == EntityTypes.PrimeTcr
            ? EventTemplates.LeadTicketConvertedToPrimeTcr
            : EventTemplates.LeadTicketConvertedToResaleTcr;

        var template = _systemEventTemplates.FirstOrDefault(x => x.EventTemplateId == eventTemplate);
        var events = new List<SystemEvent>();

        if (template == null)
        {
            return;
        }

        var eventCommnet = template.EventTemplate.Replace("{{CurrentUserName}}", loggedInUserInfo.FullName);
        events.Add(new SystemEvent
        {
            ClientId = clientId,
            ContactingTypeId = 0,
            EntityId = leadTicketId,
            UserId = loggedInUserInfo.UserId,
            EventTypeId = template.EventTypeId,
            EntityTypeId = template.EntityTypeId,
            EventCreationDateTime = DateTime.UtcNow,
            EventCreationDateTimeNumeric = long.Parse(DateTime.UtcNow.ToString("yyyyMMddHHmmss")),
            EventDateTime = DateTime.UtcNow,
            EventDateTimeNumeric = long.Parse(DateTime.UtcNow.ToString("yyyyMMddHHmmss")),
            EventProcessId = template.EventProcessId,
            EventComment = eventCommnet,
            Dismissed = false,
            EventSource = false,
            FromUserId = 0,
            Impersonated = false,
            IsConverted = false,
            OriginalUserId = loggedInUserInfo.UserId,
            ToUserId = 0
        });

        await repo.CreateEventsAsync(events);
        await repo.UpdateEntityEventsAsync(EntityTypes.LeadTicket, leadTicketId);
    }

    public async Task GenerateNewPrimeTcrSystemEventsAsync(long primeTcrId, long clientId, bool isRecContracted, long leadTicketId)
    {

        var template = _systemEventTemplates.FirstOrDefault(x => x.EventTemplateId == EventTemplates.PrimeTcrCreate);
        var events = new List<SystemEvent>();

        if (template != null)
        {

            var eventCommnet = template.EventTemplate.Replace("{{CurrentUserName}}", loggedInUserInfo.FullName);

            events.Add(new SystemEvent
            {
                ClientId = clientId,
                ContactingTypeId = 0,
                EntityId = primeTcrId,
                UserId = loggedInUserInfo.UserId,
                EventTypeId = template.EventTypeId,
                EntityTypeId = template.EntityTypeId,
                EventCreationDateTime = DateTime.UtcNow,
                EventCreationDateTimeNumeric = long.Parse(DateTime.UtcNow.ToString("yyyyMMddHHmmss")),
                EventDateTime = DateTime.UtcNow,
                EventDateTimeNumeric = long.Parse(DateTime.UtcNow.ToString("yyyyMMddHHmmss")),
                EventProcessId = template.EventProcessId,
                EventComment = eventCommnet,
                Dismissed = false,
                EventSource = false,
                FromUserId = 0,
                Impersonated = false,
                IsConverted = false,
                OriginalUserId = loggedInUserInfo.UserId,
                ToUserId = 0
            });
        }
        var createTemplate = isRecContracted
            ? EventTemplates.PrimeTcrSetContracted
            : EventTemplates.PrimeTcrSetReserved;

        template = _systemEventTemplates.FirstOrDefault(x => x.EventTemplateId == EventTemplates.PrimeTcrCreate);

        if (template != null)
        {
            var eventCommnet = template.EventTemplate.Replace("{{CurrentUserName}}", loggedInUserInfo.FullName);

            events.Add(new SystemEvent
            {
                ClientId = clientId,
                ContactingTypeId = 0,
                EntityId = primeTcrId,
                UserId = loggedInUserInfo.UserId,
                EventTypeId = template.EventTypeId,
                EntityTypeId = template.EntityTypeId,
                EventCreationDateTime = DateTime.UtcNow,
                EventCreationDateTimeNumeric = long.Parse(DateTime.UtcNow.ToString("yyyyMMddHHmmss")),
                EventDateTime = DateTime.UtcNow,
                EventDateTimeNumeric = long.Parse(DateTime.UtcNow.ToString("yyyyMMddHHmmss")),
                EventProcessId = template.EventProcessId,
                EventComment = eventCommnet,
                Dismissed = false,
                EventSource = false,
                FromUserId = 0,
                Impersonated = false,
                IsConverted = false,
                OriginalUserId = loggedInUserInfo.UserId,
                ToUserId = 0
            });
        }

        if (events.Count == 0)
        {
            return;

        }
        await repo.CreateEventsAsync(events);
        await repo.UpdateEntityEventsAsync(EntityTypes.PrimeTcr, primeTcrId, 0, leadTicketId);

    }

    public async Task GeneratePrimeTcrsActionsEventsAsync(List<PrimeTcr> affectedPrimeTcrs,
        PrimeTcrEntityActionModel model)
    {




        var events = new List<SystemEvent>();
        var contactingTypeId = 0;
        var eventTypeId = (int)EventTypes.SystemGenerated;
        var entityTypeId = (int)EntityTypes.PrimeTcr;
        var eventProcessId = model.EventProcess;
        var eventComment = model.EventComment;

        foreach (var affectedPrimeTcr in affectedPrimeTcrs)
        {
            var fromUser = 0;
            var toUser = 0;



            events.Add(new SystemEvent
            {
                ClientId = affectedPrimeTcr.ClientId,
                ContactingTypeId = contactingTypeId,
                EntityId = affectedPrimeTcr.Id,
                UserId = loggedInUserInfo.UserId,
                EventTypeId = eventTypeId,
                EntityTypeId = entityTypeId,
                EventCreationDateTime = DateTime.UtcNow,
                EventCreationDateTimeNumeric = long.Parse(DateTime.UtcNow.ToString("yyyyMMddHHmmss")),
                EventDateTime = DateTime.UtcNow,

                EventDateTimeNumeric = long.Parse(DateTime.UtcNow
 .ToString("yyyyMMddHHmmss")),
                EventProcessId = eventProcessId,
                EventComment = eventComment,
                Dismissed = false,
                EventSource = false,
                FromUserId = fromUser,
                Impersonated = false,
                IsConverted = false,
                OriginalUserId = loggedInUserInfo.UserId,
                ToUserId = toUser
            });
        }

        await repo.CreateBulkEventsAsync(events);

        foreach (var affectedPrimeTcr in affectedPrimeTcrs)
        {
            await repo.UpdateEntityEventsAsync(EntityTypes.PrimeTcr, affectedPrimeTcr.Id, 0, affectedPrimeTcr.LeadTicketId);
        }
    }


    public async Task GenerateNewCallEventsAsync(CallCreateModel model)
    {
        //create call
        var template = _systemEventTemplates.FirstOrDefault(x => x.EventTemplateId == EventTemplates.CallCreate);
        var events = new List<SystemEvent>();

        if (template != null)
        {
            events.Add(new SystemEvent
            {
                ClientId = model.ClientId,
                ContactingTypeId = 0,
                EntityId = model.CallId,
                UserId = loggedInUserInfo.UserId,
                EventTypeId = template.EventTypeId,
                EntityTypeId = template.EntityTypeId,
                EventCreationDateTime = DateTime.UtcNow,
                EventCreationDateTimeNumeric = long.Parse(DateTime.UtcNow.ToString("yyyyMMddHHmmss")),
                EventDateTime = DateTime.UtcNow,
                EventDateTimeNumeric = long.Parse(DateTime.UtcNow.ToString("yyyyMMddHHmmss")),
                EventProcessId = template.EventProcessId,
                EventComment = GetEventComment(template, model),
                Dismissed = false,
                EventSource = false,
                FromUserId = 0,
                Impersonated = false,
                IsConverted = false,
                OriginalUserId = loggedInUserInfo.UserId,
                ToUserId = 0
            });
        }

        if (model.BranchId > 0)
        {
            template = _systemEventTemplates.FirstOrDefault(x =>
                x.EventTemplateId == EventTemplates.CallAssignToBranch);

            if (template != null)
            {
                events.Add(new SystemEvent
                {
                    ClientId = model.ClientId,
                    ContactingTypeId = 0,
                    EntityId = model.CallId,
                    UserId = loggedInUserInfo.UserId,
                    EventTypeId = template.EventTypeId,
                    EntityTypeId = template.EntityTypeId,
                    EventCreationDateTime = DateTime.UtcNow,
                    EventCreationDateTimeNumeric = long.Parse(DateTime.UtcNow.ToString("yyyyMMddHHmmss")),
                    EventDateTime = DateTime.UtcNow,
                    EventDateTimeNumeric = long.Parse(DateTime.UtcNow.ToString("yyyyMMddHHmmss")),
                    EventProcessId = template.EventProcessId,
                    EventComment = GetEventComment(template, model),
                    Dismissed = false,
                    EventSource = false,
                    FromUserId = 0,
                    Impersonated = false,
                    IsConverted = false,
                    OriginalUserId = loggedInUserInfo.UserId,
                    ToUserId = 0
                });
            }
        }

        await repo.CreateEventsAsync(events);
        await repo.UpdateEntityEventsAsync(EntityTypes.Call, model.CallId);
    }

    protected override UserPrivilegeItem? CheckPrivilege(SystemPrivileges privilege)
    {
        return loggedInUserInfo.Privileges.FirstOrDefault(z =>
            z.Privilege == privilege && z.PrivilegeScope != PrivilegeScopes.Denied);
    }

    private string GetEventComment<T>(SystemEventTemplate template, T model)
    {
        return UtilityFunctions.ReplaceSpaceHolders(template.EventTemplate, template.AnchorsJson, model);
    }

    protected override void PopulateInitialData()
    {
        if (repo.CurrentUserId == 0)
        {
            repo.CurrentUserBranchId = loggedInUserInfo.BranchId;
            repo.CurrentUserCompanyId = loggedInUserInfo.CompanyId;
            repo.CurrentUserId = loggedInUserInfo.UserId;
        }

        if (_systemEventTemplates.Any() == false)
        {
            PopulateSystemEventTemplates();
        }
    }

    private void PopulateSystemEventTemplates()
    {
        var str = cache.GetItem(nameof(SystemEventTemplate));

        if (string.IsNullOrEmpty(str) == false)
        {
            var tempArray = UtilityFunctions.DeserializeJsonString<SystemEventTemplate[]>(str);

            if (tempArray.Any())
            {
                _systemEventTemplates = tempArray;

                return;
            }
        }

        _systemEventTemplates = repo.GetSystemEventTemplatesAsync();
        cache.SaveItem(nameof(SystemEventTemplate), UtilityFunctions.SerializeToJsonString(_systemEventTemplates));
    }
}
using CloudBlue.Domain.DataModels.Crm;
using CloudBlue.Domain.DataModels.PrimeTcrs;
using CloudBlue.Domain.DomainModels;
using CloudBlue.Domain.DomainModels.CallLeads;
using CloudBlue.Domain.Enums;

namespace CloudBlue.Domain.Interfaces.Services;

public interface ISystemEventsService : IBaseService
{
    Task GenerateNewCallEventsAsync(CallCreateModel model);

    Task AddEventAsync(long entityId, EntityTypes entityType, EventTypes eventType, long clientId, int userId,
        string comment, EventProcesses eventProcess, int contactingTypeId);
    Task GenerateNewLeadTicketEventsAsync(LeadTicketCreateModel model);
    Task GenerateLeadTicketsActionsEventsAsync(List<LeadTicket> affectedLeads, LeadTicketActionModel action);
    Task GenerateLeadTicketsConvertedToTcrAsync(long leadTicketId, EntityTypes tcrType, long clientId);
    Task GenerateNewPrimeTcrSystemEventsAsync(long primeTcrId, long clientId, bool isRecContracted, long leadTicketId);
    Task GeneratePrimeTcrsActionsEventsAsync(List<PrimeTcr> affectedPrimeTcrs, PrimeTcrEntityActionModel model);
}
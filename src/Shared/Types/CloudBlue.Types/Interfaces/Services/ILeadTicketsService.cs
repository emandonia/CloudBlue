using CloudBlue.Domain.DomainModels.CallLeads;
using CloudBlue.Domain.DomainModels.Filtration.Interfaces;
using CloudBlue.Domain.Enums;
using CloudBlue.Domain.GenericTypes;

namespace CloudBlue.Domain.Interfaces.Services;

public interface ILeadTicketsService : IBaseService
{
    Task<bool> CreateLeadTicketAsync(LeadTicketCreateModel model);
    Task<bool> CreateLeadTicketFromCallAsync(CallCreateModel call);
    Task<ListResult<LeadTicketItemForList>> GetLeadTicketsAsync(LeadTicketsFiltersModel filters);
    Task<LeadTicketInfoItemForTcr?> GetLeadTicketForPrimeTcrAsync(long id);
    Task UpdateLeadTicketTcrStatusAsync(List<long> leadTicketIds, int status, EntityTypes tcrType);
    Task UpdateLeadTicketViewedByAgentAsync(long id);
}
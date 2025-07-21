using CloudBlue.Domain.DomainModels;

namespace CloudBlue.Domain.Interfaces.Services;

public interface ILeadTicketsActionsService : IBaseService
{
    Task<List<EntityActionResult>> ApplyActionAsync(LeadTicketActionModel model);
}
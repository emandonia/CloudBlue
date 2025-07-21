using CloudBlue.Domain.DataModels.Crm;
using CloudBlue.Domain.DomainModels;
using CloudBlue.Domain.DomainModels.CallLeads;
using CloudBlue.Domain.DomainModels.Filtration.Interfaces;
using CloudBlue.Domain.GenericTypes;
using CloudBlue.Domain.Interfaces.Services;

namespace CloudBlue.Domain.Interfaces.Repositories;

public interface ILeadTicketsRepository : IBaseRepository
{
    Task<bool> CreateLeadTicketAsync(LeadTicketCreateModel model);
    Task<LeadTicketInfoItemForTcr?> GetLeadTicketForPrimeTcrAsync(long id);

    Task<ListResult<LeadTicketItemForList>> GetLeadTicketsAsync(LeadTicketsFiltersModel filters,
        ILookUpsService lookUpsService);

    Task<List<LeadTicket>> GetLeadTicketEntitiesAsync(List<long> itemsIds, bool fullItemsOnly,
        bool includeExtendedEntity);

    Task UpdateEntitiesAsync(IEnumerable<LeadTicket> entities);
    Task<bool> CheckTcrExistsAsync(long id);
    Task<List<LeadTicketInfoForEmail>> GetLeadTicketForEmailsAsync(long[] affectedIds);

}
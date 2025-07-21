using CloudBlue.Domain.DomainModels.CallLeads;
using CloudBlue.Domain.DomainModels.Filtration.Interfaces;
using CloudBlue.Domain.GenericTypes;

namespace CloudBlue.Domain.Interfaces.Repositories;

public interface ICallsRepository : IBaseRepository
{
    Task<ListResult<CallItemForList>> GetCalls(CallsFiltersModel callsFilters);
    Task<bool> CreateCallAsync(CallCreateModel callCreateModel);
}
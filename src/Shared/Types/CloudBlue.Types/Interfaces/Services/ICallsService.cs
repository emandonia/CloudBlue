using CloudBlue.Domain.DomainModels.CallLeads;
using CloudBlue.Domain.DomainModels.Filtration.Interfaces;
using CloudBlue.Domain.GenericTypes;

namespace CloudBlue.Domain.Interfaces.Services;

public interface ICallsService : IBaseService
{
    Task<bool> CreateCallAsync(CallCreateModel callCreateModel);
    Task<ListResult<CallItemForList>> GetCallsAsync(CallsFiltersModel callsFilters);
}
using CloudBlue.Domain.DomainModels.Filtration.Interfaces;
using CloudBlue.Domain.DomainModels.PrimeTcrs;
using CloudBlue.Domain.GenericTypes;

namespace CloudBlue.Domain.Interfaces.Services;

public interface IPrimeTcrsService : IBaseService
{
    Task<bool> CreatePrimeTcrAsync(CreatePrimeTcrModel model);
    Task<ListResult<PrimeTcrItemForList>> GetPrimeTcrsAsync(PrimeTcrsFiltersModel filters);
    Task<PrimeTcrFullItem?> GetPrimeTcrForViewAsync(long id);
    Task<bool> UpdatePrimeTcrConfigsAsync(PrimeTcrFullItem model);
    Task<bool> UpdatePrimeTcrAgentTreeAsync(long primeTcrId, int agentId);
}
using CloudBlue.Domain.DataModels.PrimeTcrs;
using CloudBlue.Domain.DomainModels.Filtration.Interfaces;
using CloudBlue.Domain.DomainModels.PrimeTcrs;
using CloudBlue.Domain.GenericTypes;

namespace CloudBlue.Domain.Interfaces.Repositories;

public interface IPrimeTcrsRepository : IBaseRepository
{
    Task<bool> CreatePrimeTcrAsync(CreatePrimeTcrFullModel model);
    Task<ListResult<PrimeTcrItemForList>> GetPrimeTcrsAsync(PrimeTcrsFiltersModel filters);
    Task<long> CheckUnitNumberExistAsync(string unitNumber, int projectId, int[] statusesToExclude);
    Task<PrimeTcrFullItem?> GetSinglePrimeTcrAsync(PrimeTcrItemFiltersModel filter);
    Task<PrimeTcr[]> GetPrimeTcrsEntitiesAsync(List<long> modelItemsIds);
    Task UpdateEntitiesAsync(List<PrimeTcr> affectedPrimeTcrs);
    Task SaveAttachmentAsync(PrimeTcrAttachment attachment);
}
using CloudBlue.Domain.DomainModels;

namespace CloudBlue.Domain.Interfaces.Services;
public interface IPrimeTcrsActionsService : IBaseService
{
    Task<List<EntityActionResult>> ApplyActionAsync(PrimeTcrEntityActionModel model);
}
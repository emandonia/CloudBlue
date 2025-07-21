using CloudBlue.Domain.DomainModels;
using CloudBlue.Domain.DomainModels.Filtration.Interfaces;
using CloudBlue.Domain.DomainModels.Users;
using CloudBlue.Domain.GenericTypes;

namespace CloudBlue.Domain.Interfaces.Services;

public interface IPrivilegesService : IBaseService
{
    Task<ListResult<EntityPrivilegeItemForList>> GetEntityPrivilegesAsync(EntityPrivilegesFiltersModel filters);
    Task<EntityPrivilegeItem[]> GetAllEntityPrivilegesAsync();

    Task<bool> CreateEntityPrivilegeAsync(EntityPrivilegeModel model);
    Task<bool> DeletePrivilegeAsync(long id);
}
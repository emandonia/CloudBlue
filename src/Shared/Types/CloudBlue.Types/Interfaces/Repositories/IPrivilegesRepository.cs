using CloudBlue.Domain.DomainModels;
using CloudBlue.Domain.DomainModels.Filtration.Interfaces;
using CloudBlue.Domain.DomainModels.Users;
using CloudBlue.Domain.GenericTypes;

namespace CloudBlue.Domain.Interfaces.Repositories;

public interface IPrivilegesRepository : IBaseRepository
{
    Task<ListResult<EntityPrivilegeItemForList>> GetEntityPrivilegesAsync(EntityPrivilegesFiltersModel filters);
    Task<EntityPrivilegeItem[]> GetAllEntityPrivilegesAsync();

    Task<bool> IsPrivilegeExistingAsync(EntityPrivilegeModel model);
    Task<bool> CreatePrivilegeAsync(EntityPrivilegeModel model);
    Task<bool> DeletePrivilegeAsync(long id);
}
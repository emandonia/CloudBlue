using CloudBlue.Domain.DataModels.Lookups;

namespace CloudBlue.Domain.Interfaces.Services;

public interface IDevelopersService : IBaseService
{
    Task<ConstructionDeveloperProject[]> GetProjectsEntitiesAsync(int[] projectIds);
}
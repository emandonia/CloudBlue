using CloudBlue.Domain.DataModels.Lookups;

namespace CloudBlue.Domain.Interfaces.Repositories;

public interface IDevelopersRepository : IBaseRepository
{
    Task<ConstructionDeveloperProject[]> GetProjectsEntitiesAsync(int[] projectIds);
}
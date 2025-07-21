using CloudBlue.Domain.DataModels.Lookups;
using CloudBlue.Domain.Interfaces.DbContext;
using CloudBlue.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CloudBlue.Data.Repositories;

public class DevelopersRepository(ILookUpsDataContext appDb) : IDevelopersRepository
{
    public int CurrentUserId { get; set; }
    public int CurrentUserBranchId { get; set; }
    public int CurrentUserCompanyId { get; set; }
    public long LastCreatedItemId { get; set; }

    public async Task<ConstructionDeveloperProject[]> GetProjectsEntitiesAsync(int[] projectIds)
    {
        return await appDb.ConstructionDeveloperProjects.Include(z => z.Developer)
            .Where(z => projectIds.Contains(z.Id)).ToArrayAsync();
    }
}
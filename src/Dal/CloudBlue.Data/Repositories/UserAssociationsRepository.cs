using CloudBlue.Domain.Interfaces.DbContext;
using CloudBlue.Domain.Interfaces.Repositories;

namespace CloudBlue.Data.Repositories;

public class UserAssociationsRepository(IUsersDataContext appDb) : IUserAssociationsRepository
{
    public int CurrentUserId { get; set; }
    public int CurrentUserBranchId { get; set; }
    public int CurrentUserCompanyId { get; set; }
    public long LastCreatedItemId { get; set; }
}
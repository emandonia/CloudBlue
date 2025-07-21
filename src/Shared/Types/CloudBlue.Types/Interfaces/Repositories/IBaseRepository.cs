namespace CloudBlue.Domain.Interfaces.Repositories;

public interface IBaseRepository
{
    int CurrentUserId { set; get; }
    int CurrentUserBranchId { set; get; }
    int CurrentUserCompanyId { set; get; }
    long LastCreatedItemId { set; get; }
}
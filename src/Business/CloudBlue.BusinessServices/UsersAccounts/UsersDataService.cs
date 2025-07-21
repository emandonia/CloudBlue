using CloudBlue.Domain.BaseTypes;
using CloudBlue.Domain.DataModels.Operations;
using CloudBlue.Domain.DomainModels.Users;
using CloudBlue.Domain.DomainModels.Users.UserSessions;
using CloudBlue.Domain.DomainModels.Users.UsersManagement;
using CloudBlue.Domain.Enums;
using CloudBlue.Domain.Interfaces.Repositories;
using CloudBlue.Domain.Interfaces.Services;
using CloudBlue.Domain.Utilities;

namespace CloudBlue.BusinessServices.UsersAccounts;

public class UsersDataService(IUsersRepository repo, ICachingService cachingService) : BaseService, IUsersDataService
{
    //private UserPosition[] _positions = [];

    private UserTreeItem[] _treeItems = [];

    public async Task<UserTreeItem?> GetUserTreeAsync(int userId)
    {

        await PopulateUsersTrees();


        var item = _treeItems.FirstOrDefault(z => z.UserId == userId);

        if (item != null)
        {
            PopulateSalesUserTree(item);
        }

        return item;
    }

    public bool ForceRefresh { get; set; }

    public async Task PopulateSalesUserData(CreateUserFullModel model)
    {
        var manager = await GetUserTreeAsync(model.DirectManagerId);

        if (manager == null)
        {
            return;
        }

        model.DirectManagerName = manager.AgentName;

        if (manager.ParentId == 0 || manager.SalesUsers.SalesUsers.Count > 0)
        {
            model.TopMostManagerId = manager.UserId;
            model.TopMostManagerName = manager.AgentName;

            return;
        }

        var topManager = manager.SalesUsers.SalesUsers.OrderByDescending(z => z.Level)
            .First();

        model.TopMostManagerId = topManager.UserId;
        model.TopMostManagerName = topManager.AgentName;
    }

    protected override void PopulateInitialData()
    {
    }

    protected override UserPrivilegeItem? CheckPrivilege(SystemPrivileges privilege)
    {
        return null;
    }

    private async Task PopulateUsersTrees()
    {


        if (_treeItems.Length > 0 && ForceRefresh == false)
        {
            return;
        }

        var itemKey = nameof(SalesUserTree);
        var json = cachingService.GetItem(itemKey);

        //_positions = await repo.GetSalesUserPositionsEntitiesAsync();

        if (string.IsNullOrEmpty(json) == false)
        {


            _treeItems = UtilityFunctions.DeserializeJsonString<UserTreeItem[]>(json);


        }

        if (ForceRefresh || _treeItems.Length == 0)
        {
            _treeItems = await repo.GetSalesUserTreeItemsAsync();
            cachingService.SaveItem(itemKey, UtilityFunctions.SerializeToJsonString(_treeItems));
        }

    }

    private void PopulateSalesUserTree(UserTreeItem treeItem)
    {
        var mentorId = 4;
        var isMentor = treeItem.PositionId == mentorId;
        var firstManagerPercentage = isMentor ? (decimal)0.8 : 1;
        var isAgentOnly = treeItem.IsManager == false && treeItem.CanHaveTeam == false;
        var retList = new List<SalesUser>();
        var currentUser = treeItem.UserId;
        var level = 1;

        var currentScopedItem = _treeItems.Where(z => z.CompanyId == treeItem.CompanyId)
            .ToArray();

        if (isAgentOnly == false)
        {
            retList.Add(new SalesUser
            {
                CommissionFlatRatio = firstManagerPercentage,
                UserId = currentUser,
                Level = level,
                AgentName = treeItem.AgentName
            });

            level++;
        }

        if (treeItem.ParentId == 0)
        {
            treeItem.SalesUsers.SalesUsers = retList;

            return;
        }

        currentUser = treeItem.ParentId;

        while (true)
        {
            var parent = currentScopedItem.FirstOrDefault(z => z.UserId == currentUser);

            if (parent == null)
            {
                break;
            }

            isMentor = parent.PositionId == mentorId;
            var salesUser = new SalesUser();
            salesUser.UserId = parent.UserId;
            salesUser.AgentName = parent.AgentName;
            salesUser.Level = level;
            decimal levelRatio = 0;

            switch (level)
            {
                case 1:
                    levelRatio = isMentor ? (decimal)0.8125 : 1;

                    break;

                case 2:
                    levelRatio = (decimal).5;

                    break;

                case 3:
                    levelRatio = (decimal).3;

                    break;

                case 4:
                    levelRatio = (decimal).175;

                    break;
            }

            salesUser.CommissionFlatRatio = levelRatio;
            retList.Add(salesUser);
            currentUser = parent.ParentId;
            level++;
        }

        treeItem.SalesUsers.SalesUsers = retList.OrderBy(z => z.Level)
            .ToList();
    }
}
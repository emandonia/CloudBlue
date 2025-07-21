namespace CloudBlue.Domain.DomainModels.Users.UserSessions;

public class LoggedInUserInfo
{
    public int[] AgentsIds { set; get; } = [];

    public int BranchId { get; set; }

    public string? BranchName { get; set; }

    public bool CanHaveTeam { get; set; }

    public int CompanyId { get; set; }

    public bool CurrentAccessPrivilege { get; set; }

    public int DepartmentId { get; set; }

    public int DirectManagerId { get; set; }

    public string Email { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public bool HasLineOfBusiness { get; set; }

    public bool InResaleTeam { get; set; }

    public bool IsManager { get; set; }

    public bool IsParent { get; set; }

    public int ParentId { get; set; }

    public int PositionId { get; set; }

    public UserPrivilegeItem[] Privileges { set; get; } = [];

    public SubAccountItem[] SubAccounts { get; set; } = [];

    public int TopMostManagerId { get; set; }

    public int UserGroupId { get; set; }

    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string? UserPositionName { get; set; }
    public int[] MangersIds { get; set; } = [];
    public string? CompanyName { get; set; }
    public string? UserImagePath { get; set; }
    public int[] TeamsIds { get; set; } = [];
    public DateTime LastUpdated { get; set; }
}
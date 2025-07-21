namespace CloudBlue.Domain.DataModels.CbUsers;

public class VwAgent
{
    public int AgentId { get; set; }
    public bool IsApproved { get; set; }

    public int CompanyId { get; set; }

    public int BranchId { get; set; }

    public string AgentName { get; set; } = null!;
    public string? BranchName { get; set; }

    public int PositionId { get; set; }

    public bool InResaleTeam { get; set; }

    public int TopMostManagerId { get; set; }

    public int DirectManagerId { get; set; }

    public int UserGroupId { get; set; }

    public bool IsBranchManager { get; set; }
    public int[]? ManagersIdsArray { get; set; } = [];
    public int[]? TeamsIds { get; set; } = [];

    public string? Email { get; set; }
    public bool CanHaveTeam { get; set; }
    public string? DeviceInfo { get; set; }
}
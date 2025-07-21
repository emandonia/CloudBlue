using CloudBlue.Domain.Enums;

namespace CloudBlue.Domain.DomainModels;

public class AgentItem
{
    public string AgentName { set; get; } = null!;
    public string? AgentPhone { set; get; }
    public string? AgentEmail { set; get; }
    public SalesPersonClasses SalesPersonClass { set; get; }
    public int AgentId { set; get; }
    public int DirectManagerId { set; get; }
    public int TopMostManagerId { set; get; }
    public int CompanyId { set; get; }
    public int[]? ManagersIdsArray { get; set; } = [];
    public int[]? TeamsIds { get; set; } = [];

    public int BranchId { set; get; }
    public int PositionId { set; get; }
    public List<AgentItem> DirectTeamAgents { set; get; } = new();
    public bool CanHaveTeam { get; set; }
    public bool InResaleTeam { get; set; }
    public bool IsApproved { get; set; }
    public object? BranchName { get; set; }
}
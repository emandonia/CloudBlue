namespace CloudBlue.Domain.DomainModels.CallLeads;

public interface ITextInfos
{
    string? AgentName { get; set; }
    string? BranchName { get; set; }
    string? CompanyName { get; set; }
    string? CurrentUserName { get; set; }
}